using ExamBankSystem.Models;
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
            "填空题（每题1分，共",
            "判断题（每题1分，共",
            "简答题（每题1分，共",
            "设计题（每题1分，共",
            "编程题（每题1分，共"
        };
        public static async void ExportPaper(int paperId)
        {
            var paper = await DbHelper.GetByIdAsync<TestPaper>(paperId);
            var file = await FileHelper.SaveSingleDocxAsync(paper.Name + "试卷");
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
            for (int i = 0; i < 7; i++)
            {
                var order = 1;
                if(questionPapers[i].Count > 0)
                {
                    Paragraph paragraph = section.AddParagraph();
                    TextRange textRange = paragraph.AppendText(Title[i] + $"{questionPapers[i].Count}分）");
                    textRange.CharacterFormat.FontSize = 12;
                    textRange.CharacterFormat.Bold = true;
                }
                foreach(var item in questionPapers[i])
                {
                    Paragraph paragraph = section.AddParagraph();
                    TextRange textRange = paragraph.AppendText($"{order++}."+item.Question.Name);
                }
            }
            doc.SaveToFile(file.Path, FileFormat.WordML);
        }
    }
}
