using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Mebs_Envanter
{
    public class SelectionPersistance
    {
        static SelectionPersistance()
        {
            SerializationTool<ComboPersistantList> serializer = new SerializationTool<ComboPersistantList>();
            //SerializationTool serializer = new SerializationTool();
            //Serializer serializer = new Serializer();
            if (File.Exists(outputFile))
            {
                try
                {
                    ComboPersistantList objectToSerialize = serializer.DeSerializeObject(outputFile);
                    SortedList<String, ComboPersistantObject> objects = objectToSerialize.getList();
                    if (objects != null)
                    {
                        persistentCombos = objects;
                    }
                }
                catch (Exception) { }
            }
        }

        private static SortedList<String, ComboPersistantObject> persistentCombos =
            new SortedList<string, ComboPersistantObject>();
        private static String tempname = "tempname";
        private static String outputFileName1 = "combopersistance.xml";
        private static String outputFile = Environment.CurrentDirectory + "\\" + outputFileName1;//StaticClass.PersistanceFolder + outputFileName1;
        public static String GetMyInstanceName(DependencyObject obj)
        {
            return (String)obj.GetValue(MyInstanceNameProperty);
        }
        public static void SetMyInstanceName(DependencyObject obj, String value)
        {
            obj.SetValue(MyInstanceNameProperty, value);
        }
        public static readonly DependencyProperty MyInstanceNameProperty =
            DependencyProperty.RegisterAttached(
                "MyInstanceName",
                typeof(String),
                typeof(SelectionPersistance),
                new FrameworkPropertyMetadata(tempname, MyInstanceNameChangedCallback));


        public static void RefreshObject(DependencyObject obj)
        {
            ComboBox comboMe = (obj as ComboBox);
            int index = getPersistanceObjectValue(obj);
            try
            {
                if (index >= 0)
                {
                    comboMe.SelectedIndex = index;
                }
                else
                {
                    comboMe.SelectedIndex = 0;
                }
            }
            catch (Exception) { }
        }
        public static int getPersistanceObjectValue(DependencyObject obj)
        {
            String instanceName = GetMyInstanceName(obj);
            if (persistentCombos.ContainsKey(instanceName))
            {
                return persistentCombos[instanceName].persistantSelection;
            }
            return -1;
        }

        private static void MyInstanceNameChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            ComboBox comboMe = (d as ComboBox);
            String instanceName = GetMyInstanceName(d);
            comboMe.DataContextChanged += new DependencyPropertyChangedEventHandler(comboMe_DataContextChanged);
            comboMe.Loaded += new RoutedEventHandler(_this_Loaded);

            if (!persistentCombos.ContainsKey(instanceName))
            {
                ComboPersistantObject obj = new ComboPersistantObject();
                obj.persistantName = instanceName;
                //obj.persistantSelection = 0;
                persistentCombos.Add(instanceName, obj);
            }
            else
            {
                // Gereksiz
            }

        }


        static void _this_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboMe = (sender as ComboBox);
            comboMe.SelectionChanged += new SelectionChangedEventHandler(_this_SelectionChanged);

            SetIndexOfCombo(comboMe);
        }

        static void comboMe_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboMe = (sender as ComboBox);
            SetIndexOfCombo(comboMe);
        }

        private static void SetIndexOfCombo(ComboBox comboMe)
        {

            String instanceName = GetMyInstanceName(comboMe);
            // Here read file and set selection index
            int indexFirst = GetIndexFromFile(instanceName);
            if (indexFirst < 0) indexFirst = 0;
            if (comboMe.Items.Count > indexFirst)
            {
                comboMe.SelectedIndex = indexFirst;
            }
            else if (comboMe.Items.Count > 0)
            {
                comboMe.SelectedIndex = 0;
            }
        }

        static void _this_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Record changes.
            ComboBox comboMe = (sender as ComboBox);
            if (comboMe.SelectedIndex == -1)
            {
                //SetIndexOfCombo(comboMe);
            }
            String instanceName = GetMyInstanceName(comboMe);
            setInstanceIndexAndSave(instanceName, comboMe.SelectedIndex);
        }

        static int GetIndexFromFile(String instanceName)
        {
            if (persistentCombos != null)
            {
                if (persistentCombos.ContainsKey(instanceName))
                {
                    return persistentCombos[instanceName].persistantSelection;
                }
            }
            return 0;
        }

        static void setInstanceIndexAndSave(String instanceName, int newIndex)
        {
            if (newIndex < 0) return;
            if (persistentCombos != null && instanceName != null)
            {
                if (persistentCombos.ContainsKey(instanceName))
                {
                    persistentCombos[instanceName].persistantSelection = newIndex;
                }
            }
            SerializationTool<ComboPersistantList> serializer = new SerializationTool<ComboPersistantList>();
            //Serializer serializer = new Serializer();
            ComboPersistantList comboPersList = new ComboPersistantList();
            comboPersList.setList(persistentCombos);
            serializer.SerializeObject(outputFile, comboPersList);
        }

        [Serializable()]
        public class ComboPersistantObject : ISerializable
        {
            public String persistantName;
            public int persistantSelection = -1;
            public ComboPersistantObject() { }
            public ComboPersistantObject(SerializationInfo info, StreamingContext ctxt)
            {
                this.persistantName = (String)info.GetValue("PersistantName", typeof(String));
                this.persistantSelection = (int)info.GetValue("PersistantSelection", typeof(int));
            }
            #region ISerializable Members

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("PersistantName", this.persistantName);
                info.AddValue("PersistantSelection", this.persistantSelection);
            }
            #endregion
        }

        [Serializable()]
        public class ComboPersistantList : ISerializable
        {
            public ComboPersistantList()
            {
            }
            public ComboPersistantList(SerializationInfo info, StreamingContext ctxt)
            {
                //this.ComboOjbects = (SortedList<String, ComboPersistantObject>)
                // info.GetValue("ComboOjbects", typeof(SortedList<String, ComboPersistantObject>));               
            }
            private List<ComboPersistantObject> comboOjbects = null;
            public List<ComboPersistantObject> ComboOjbects
            {
                get { return comboOjbects; }
                set { comboOjbects = value; }
            }

            public void setList(SortedList<String, ComboPersistantObject> objs)
            {
                comboOjbects = new List<ComboPersistantObject>();
                foreach (var item in objs.Values)
                {
                    comboOjbects.Add(item);
                }
            }
            public SortedList<String, ComboPersistantObject> getList()
            {
                SortedList<String, ComboPersistantObject> retObjs = new SortedList<string, ComboPersistantObject>();
                if (comboOjbects != null)
                {
                    foreach (var item in comboOjbects)
                    {
                        if (!retObjs.ContainsKey(item.persistantName))
                        {

                            retObjs.Add(item.persistantName, item);
                        }
                    }
                }
                return retObjs;
            }

            /*private SortedList<String, ComboPersistantObject> comboOjbects;
            public SortedList<String, ComboPersistantObject> ComboOjbects
            {
                get { return this.comboOjbects; }
                set { this.comboOjbects = value; }
            }*/
            #region ISerializable Members
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                //info.AddValue("ComboOjbects", this.ComboOjbects);
            }
            #endregion
        }


    }
}
