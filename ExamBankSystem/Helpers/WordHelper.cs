using ExamBankSystem.Enums;
using ExamBankSystem.Models;
using Microsoft.UI.Xaml.Controls;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Collections.Generic;

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
                    TextRange textRange = paragraph.AppendText($"{ord++}." + Title[i] + $"{questionPapers[i].Count}分）");
                    textRange.CharacterFormat.FontSize = 14;
                    textRange.CharacterFormat.Bold = true;
                }
                foreach(var item in questionPapers[i])
                {
                    Paragraph paragraph = section.AddParagraph();
                    // paragraph.ApplyStyle(BuiltinStyle.List5);
                    TextRange textRange = paragraph.AppendText($"{order++}."+item.Question.Name);
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
                    TextRange textRange = paragraph.AppendText($"{ord++}." + Title[i] + $"{questionPapers[i].Count}分）");
                    textRange.CharacterFormat.FontSize = 14;
                    textRange.CharacterFormat.Bold = true;
                }
                foreach (var item in questionPapers[i])
                {
                    Paragraph paragraph = section.AddParagraph();
                    TextRange textRange = paragraph.AppendText($"{order++}.");
                    paragraph.AppendText(item.Question.Answer);
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
    }
        
}
