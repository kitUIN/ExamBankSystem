using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ExamBankSystem.Helpers
{
    public class FileHelper
    {
        public static async Task<StorageFile> GetSingleDocxAsync()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add(".doc");
            openPicker.FileTypeFilter.Add(".docx");
            return await openPicker.PickSingleFileAsync();
        }
        public static async Task<List<string>> GetMultipleDocxAsync()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            FileOpenPicker openPicker = new FileOpenPicker();
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
            openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openPicker.FileTypeFilter.Add(".doc");
            openPicker.FileTypeFilter.Add(".docx");
            var res = new List<string>();
            foreach(var i in await openPicker.PickMultipleFilesAsync())
            {
                res.Add(i.Path);
            }
            return res;
        }
        public static async Task<StorageFile> SaveSingleDocxAsync(string suggestedFileName = "New Document")
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            FileSavePicker savePicker = new FileSavePicker();
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Word", new List<string>() { ".doc",".docx" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = suggestedFileName;
            return await savePicker.PickSaveFileAsync();
        }
    }
}
