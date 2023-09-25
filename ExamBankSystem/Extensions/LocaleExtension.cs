using Microsoft.UI.Xaml.Markup;
namespace ShadowViewer.Extensions
{
    /// <summary>
    /// �����Ա��ػ�
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal sealed class LocaleExtension : MarkupExtension
    {
        
        /// <summary>
        /// ��ֵ
        /// </summary>
        public CoreResourceKey Key { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return CoreResourcesHelper.GetString(Key);
        }
    }
}