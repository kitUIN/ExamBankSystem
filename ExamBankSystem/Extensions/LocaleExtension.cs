using Windows.UI.Xaml.Markup;
using ExamBankSystem.Enums;
using ExamBankSystem.Helpers;

namespace ExamBankSystem.Extensions
{
    /// <summary>
    /// 本地化拓展
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal sealed class LocaleExtension : MarkupExtension
    {
        
        /// <summary>
        /// 本地化键
        /// </summary>
        public ResourceKey Key { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return ResourcesHelper.GetString(Key);
        }
    }
}