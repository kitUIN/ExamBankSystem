using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ExamBankSystem.Helpers
{
    public class FileHelper
    {
        public static async Task<StorageFile> GetDocxAsync()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".doc");
            picker.FileTypeFilter.Add(".docx");
            return await picker.PickSingleFileAsync();
        }
    }
}
