using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using ExamBankSystem.Utils;
using Microsoft.UI.Xaml.Controls;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExamBankSystem.Helpers
{
    public class WordHelper
    {
        public static string[] Title = new string[7]
        {
            "选择题（每题1分，共",
            "多选题（每题1分，共",
            "填空题（共",
            "判断题（每题1分，共",
            "简答题（共",
            "设计题（共",
            "编程题（共"
        };
        public static string[] TitleOrder = new string[7]
        {
            "一",
            "二",
            "三",
            "四",
            "五",
            "六",
            "七"
        };
        public static async void ExportPaper(int paperId,string name = "试卷")
        {
            var paper = await DbHelper.GetByIdAsync<TestPaper>(paperId);
            var file = await FileHelper.SaveSingleDocxAsync(paper.Name + name);
            var questions = DbHelper.GetQuestionsPapersByTestPaper(paperId);
            List<List<QuestionPaper>> questionPapers = new List<List<QuestionPaper>>();
            for (int i = 0; i < 7; i++)
            {
                questionPapers.Add(new List<QuestionPaper>());
            }
            foreach (var question in questions)
            {
                questionPapers[(int) question.Question.Type].Add(question);
            }
            var doc = new Document();
            Section section = doc.AddSection();
            var ord = 1;
            for (int i = 0; i < 7; i++)
            {
                var order = 1;
                if(questionPapers[i].Count > 0)
                {
                    Paragraph paragraph = section.AddParagraph(); 
                    TextRange textRange = paragraph.AppendText($"{TitleOrder[i]}、" + Title[i] + $"{questionPapers[i].Sum(x => x.Question.Point)}分）");
                    textRange.CharacterFormat.FontSize = 14;
                    textRange.CharacterFormat.Bold = true;
                }
                foreach(var item in questionPapers[i])
                {
                    Paragraph paragraph = section.AddParagraph(); 
                    TextRange textRange = paragraph.AppendText($"{order++}."+item.Question.Name);
                    if (i != 0 && i != 1 && i != 3)
                    {
                        paragraph.AppendText($"({item.Question.Point}分)");
                    }
                }
            }
            if (file == null)
            {
                EventHelper.InvokeTipPopup(null,
                                        ResourcesHelper.GetString(ResourceKey.ExpoertError),
                                        InfoBarSeverity.Error
                                    );
                return;
            }
            doc.SaveToFile(file.Path, FileFormat.WordML);
            EventHelper.InvokeTipPopup(null,
                                        ResourcesHelper.GetString(ResourceKey.ExpoertSuccess),
                                        InfoBarSeverity.Success
                                    );
        }

        public static async void ExportAnswer(int paperId, string name = "答案")
        {
            var paper = await DbHelper.GetByIdAsync<TestPaper>(paperId);
            var file = await FileHelper.SaveSingleDocxAsync(paper.Name + "答案");
            var questions = DbHelper.GetQuestionsPapersByTestPaper(paperId);
            List<List<QuestionPaper>> questionPapers = new List<List<QuestionPaper>>();
            for (int i = 0; i < 7; i++)
            {
                questionPapers.Add(new List<QuestionPaper>());
            }
            foreach (var question in questions)
            {
                questionPapers[(int)question.Question.Type].Add(question);
            }
            var doc = new Document();
            Section section = doc.AddSection();
            var ord = 1;
            for (int i = 0; i < 7; i++)
            {
                var order = 1;
                if (questionPapers[i].Count > 0)
                {
                    Paragraph paragraph = section.AddParagraph();
                    TextRange textRange = paragraph.AppendText($"{TitleOrder[i]}、" + Title[i] + $"{questionPapers[i].Sum(x=>x.Question.Point)}分）");
                    textRange.CharacterFormat.FontSize = 14;
                    textRange.CharacterFormat.Bold = true;
                }
                Paragraph paragraph1 = section.AddParagraph();
                foreach (var item in questionPapers[i])
                {
                    if (i > 3)
                    {
                        paragraph1 = section.AddParagraph();
                    }
                    TextRange textRange = paragraph1.AppendText($"{order++}.");
                    paragraph1.AppendText(item.Question.Answer);
                    if (i <= 3)
                    {
                        paragraph1.AppendText("    ");
                    } 
                }
            }
            if (file == null)
            {
                EventHelper.InvokeTipPopup(null,
                                        ResourcesHelper.GetString(ResourceKey.ExpoertError),
                                        InfoBarSeverity.Error
                                    );
                return;
            }
            doc.SaveToFile(file.Path, FileFormat.WordML);
            EventHelper.InvokeTipPopup(null,
                                        ResourcesHelper.GetString(ResourceKey.ExpoertSuccess),
                                        InfoBarSeverity.Success
                                    );
        }

        public static List<Question> ImportPaper(string path)
        {
            Regex regex = new Regex(@"[\(\（](\d+)分[\)）]", RegexOptions.IgnoreCase);
            var res = new List<Question>();
            Document doc = new Document();
            doc.LoadFromFile(path);
            foreach (Section section in doc.Sections)
            {
                var current = 0;
                int j = 0;
                var tempText = "";
                for (int i = 0; i < section.Paragraphs.Count; i++)
                {
                    Paragraph paragraph = section.Paragraphs[i];
                    if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "选择题" || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("选择题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 0;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "多选题"|| paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("多选题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        Console.WriteLine(tempText);
                        current = 1;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "填空题" || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("填空题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 2;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "判断题" || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("判断题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 3;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "简答题" || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("简答题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 4;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "设计题" || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("设计题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 5;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && (paragraph.Text.Substring(2, 3) == "编程题" || paragraph.Text.Substring(2, 3) == "程序题") || paragraph.Text.Length >= 3 && paragraph.Text.TrimStart().StartsWith("编程题"))
                    {
                        if (tempText != "")
                        {
                            var point = 1;
                            if (current != 0 && current != 1 && current != 3)
                            {
                                var groups = regex.Match(tempText).Groups;
                                if (groups[0].Success)
                                {
                                    point = Convert.ToInt32(groups[1].Value);
                                    tempText.Replace(groups[0].Value, "");
                                }
                            }
                            res.Add(new Question()
                            {
                                Name = tempText,
                                Answer = "",
                                Rank = 1,
                                Point = point,
                                SubjectId = 0,
                                KnowledgeId = 0,
                                Type = (QuestionType)current,
                                UploadUserId = CurrentData.CurrentUser.Id,
                            });
                        }
                        current = 6;
                        j = 1;
                        tempText = "";
                    }
                    else
                    {
                        if (paragraph.Text.StartsWith($"{j}."))
                        {
                            if (tempText != "")
                            {
                                var point = 1;
                                if (current != 0 && current != 1 && current != 3)
                                {
                                    var groups = regex.Match(tempText).Groups;
                                    if (groups[0].Success)
                                    {
                                        point = Convert.ToInt32(groups[1].Value);
                                        tempText.Replace(groups[0].Value, "");
                                    }
                                }
                                res.Add(new Question()
                                {
                                    Name = tempText,
                                    Answer = "",
                                    Rank = 1,
                                    Point = point,
                                    SubjectId = 0,
                                    KnowledgeId = 0,
                                    Type = (QuestionType)current,
                                    UploadUserId = CurrentData.CurrentUser.Id,
                                });
                            }
                            tempText = "";
                            tempText += paragraph.Text.Substring(2);
                            j++;
                        }
                        else
                        {
                            tempText += "\n" + paragraph.Text;
                        }
                    }
                    
                }
            }
            return res;
        }
        public static List<string> ImportAnswer(string path)
        {
            var res = new List<string>();
            Document doc = new Document();
            doc.LoadFromFile(path);
            foreach (Section section in doc.Sections)
            {
                var current = 0;
                int j = 0;
                var tempText = "";
                for (int i = 0; i < section.Paragraphs.Count; i++)
                {
                    Paragraph paragraph = section.Paragraphs[i];
                    if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "选择题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 0;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "多选题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        Console.WriteLine(tempText);
                        current = 1;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "填空题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 2;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "判断题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 3;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "简答题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 4;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && paragraph.Text.Substring(2, 3) == "设计题")
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 5;
                        j = 1;
                        tempText = "";
                    }
                    else if (paragraph.Text.Length >= 5 && (paragraph.Text.Substring(2, 3) == "编程题" || paragraph.Text.Substring(2, 3) == "程序题"))
                    {
                        if (tempText != "")
                        {
                            res.Add(tempText.Trim());
                        }
                        current = 6;
                        j = 1;
                        tempText = "";
                    }
                    else
                    {
                        if (paragraph.Text.StartsWith($"{j}."))
                        {

                            int l = j;
                            while (true)
                            {
                                if (paragraph.Text.Contains(l.ToString()))
                                {
                                    l++;
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (l != j + 1)
                            {
                                var tt = paragraph.Text.Substring(2);

                                for (int k = j + 1; k <= l; k++)
                                {
                                    var ttt = tt.Split($"{k}.");
                                    if (ttt.Length == 0)
                                    {
                                        ttt = tt.Split($"{k} .");
                                    }

                                    if (ttt.Length==1)
                                    {
                                        if (ttt[0].Trim() != "")
                                        {
                                            res.Add(ttt[0].Trim());
                                        }
                                        break;
                                    }
                                    tt = ttt[1];
                                    if (ttt[0].Trim() != "")
                                    {
                                        res.Add(ttt[0].Trim());
                                    }
                                }
                            }
                            else
                            {
                                if (tempText != "")
                                {
                                    res.Add(tempText);
                                    tempText = paragraph.Text.Substring(2);
                                }
                            }
                            j = l;
                        }
                        else
                        {
                            tempText += "\n" + paragraph.Text;
                        }
                    }

                }
            }
            return res;
        }
    }
        
}
