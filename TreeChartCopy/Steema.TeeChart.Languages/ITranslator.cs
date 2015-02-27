namespace Steema.TeeChart.Languages
{
    using System;
    using System.Windows.Forms;

    public interface ITranslator
    {
        bool AskLanguage(ref int language);
        bool HasUpperCase();
        void InitLanguage(int language);
        string LanguageToString(int language);
        void Translate(Control c);
    }
}

