namespace Steema.TeeChart.Data
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class TextFieldCollection : CollectionBase
    {
        internal TextFieldCollection()
        {
        }

        public int Add(TextField field)
        {
            return base.List.Add(field);
        }

        public int Add(int index, string name)
        {
            return this.Add(new TextField(index, name));
        }

        public void AddRange(TextField[] fields)
        {
            foreach (TextField field in fields)
            {
                base.List.Add(field);
            }
        }

        public void Insert(int index, TextField field)
        {
            if (base.List.IndexOf(field) == -1)
            {
                base.List.Insert(index, field);
            }
        }

        public void Remove(TextField field)
        {
            base.List.Remove(field);
        }

        public TextField this[int index]
        {
            get
            {
                return (TextField) base.List[index];
            }
        }
    }
}

