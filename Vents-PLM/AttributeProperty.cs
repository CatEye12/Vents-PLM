﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Vents_PLM
{
        class AttributeProperty
        {
            String m_GUID;
            [Browsable(true)]
            [ReadOnly(true)]
            [Description("Уникальный глобальный идентификатор атрибута")]
            [Category("Идентификация")]
            [DisplayName("Глобальный идентификатор")]
            public String GUID
            {
                get { return m_GUID; }
                set { m_GUID = value; }
            }


            String m_ATTRIBUTE_ID;
            [Browsable(true)]
            [ReadOnly(true)]
            [Description("Локальный идентификатор атрибута. Назначается системой автоматически и не подлежит дальнейшей модификации")]
            [Category("Идентификация")]
            [DisplayName("Идентификатор")]
            public String ATTRIBUTE_ID
            {
                get { return m_ATTRIBUTE_ID; }
                set { m_ATTRIBUTE_ID = value; }
            }


            String m_NOTE;
            [Browsable(true)]
            [Description("Позволяет ввести комментарий для данного атрибута, поясняющий его назначение. Пользователь сможет увидеть данный комментарий в карточке объекта, выделив атрибут в списке атрибутов объекта или связи на закладке Свойства или Свойства связей.")]
            [Category("Идентификация")]
            [DisplayName("Комментарий")]
            public String NOTE
            {
                get { return m_NOTE; }
                set { m_NOTE = value; }
            }


            String m_SHORT_NAME;
            [Browsable(true)]
            [Description("Краткое наименование данного атрибута (до 32 символов). Может содержать пустые и неуникальные значения. Используется для отображения в таблицах с различной справочной информацией, упрощенного отображения формул в экспертной системе и т.д.")]
            [Category("Идентификация")]
            [DisplayName("Краткое наименование")]
            public String SHORT_NAME
            {
                get { return m_SHORT_NAME; }
                set { m_SHORT_NAME = value; }
            }


            String m_NAME;
            [Browsable(true)]
            [Description("Позволяет ввести уникальное наименование атрибута (до 255 символов)")]
            [Category("Идентификация")]
            [DisplayName("Наименование")]
            public String NAME
            {
                get { return m_NAME; }
                set { m_NAME = value; }
            }


            String m_ALIAS;
            [Browsable(true)]
            [Description("Позволяет ввести Alias")]
            [Category("Идентификация")]
            [DisplayName("Псевдоним")]
            public String ALIAS
            {
                get { return m_ALIAS; }
                set { m_ALIAS = value; }
            }


            string  m_List;
            [Browsable(true)]
            [Description("Позволяет определить режим работы со списковыми параметрами")]
            [Category("Тип хранимой информации")]
            [DisplayName("Список")]
            [TypeConverter(typeof(StringListConverter))]
            public string  List
            {
                get { return m_List; }
                set { m_List = value; }
            }


        public AttributeProperty()
        {
            m_NAME = "Атрибут номер 1";
            GUID = Guid.NewGuid().ToString();
            m_List = StringListConverter.s0;
        }

        public void SaveAttribute(AttributeProperty property)
        {
            SqlDataReader reader;
            SqlCommand save = new SqlCommand();
            save.Connection = SQLConnection.con;
            save.Connection.Open();
            save.CommandType = System.Data.CommandType.StoredProcedure;
            save.CommandText = "IMS_ADD_ATTRIBUTE";
            

            save.Parameters.AddWithValue("inNAME", property.m_NAME);
            save.Parameters.AddWithValue("inSHORT_NAME", property.m_SHORT_NAME ?? DBNull.Value.ToString());
            save.Parameters.AddWithValue("inALIAS", property.m_ALIAS ?? DBNull.Value.ToString());
            save.Parameters.AddWithValue("inGUID", property.m_GUID);
            int mulID = 0;
            switch (property.m_List)
            {
                case "Атрибут может содержать одно значение":
                    mulID = 0;
                    break;
                case "Атрибут может содержать множество значений":
                    mulID = 1;
                    break;
                case "Атрибут может содержать одно значение из списка разрешённых значений+":
                    mulID = 2;
                    break;
                case "Атрибут может содержать множество значений из списка":
                    mulID = 3;
                    break;
            }

            save.Parameters.AddWithValue("inNOTE", property.m_NOTE ?? DBNull.Value.ToString());
            save.Parameters.AddWithValue("inMULTIPLE_VALUED", mulID);
            reader = save.ExecuteReader();
            reader.Close();
            save.Connection.Close();
        }
        public void UpdateAttribute(AttributeProperty property)
        {
            SqlDataReader reader;
            SqlCommand save = new SqlCommand();
            save.Connection = SQLConnection.con;
            save.Connection.Open();
            save.CommandType = System.Data.CommandType.StoredProcedure;
            save.CommandText = "IMS_UPDATE_ATTRIBUTE";
            save.Parameters.AddWithValue("inNAME", property.m_NAME);
            save.Parameters.AddWithValue("inSHORT_NAME", property.m_SHORT_NAME ?? DBNull.Value.ToString());
            save.Parameters.AddWithValue("inALIAS", property.m_ALIAS ?? DBNull.Value.ToString());
            save.Parameters.AddWithValue("inGUID", property.m_GUID);
            save.Parameters.AddWithValue("inNOTE", property.m_NOTE ?? DBNull.Value.ToString());
            int mulID = 0;
            switch (property.m_List)
            {
                case "Атрибут может содержать одно значение":
                    mulID = 0;
                    break;
                case "Атрибут может содержать множество значений":
                    mulID = 1;
                    break;
                case "Атрибут может содержать одно значение из списка разрешённых значений+":
                    mulID = 2;
                    break;
                case "Атрибут может содержать множество значений из списка":
                    mulID = 3;
                    break;
            }
            
            save.Parameters.AddWithValue("inMULTIPLE_VALUED", mulID);
            reader = save.ExecuteReader();
            reader.Close();
            save.Connection.Close();
        }
        public void DeleteAttribute(AttributeProperty property)
        {
            SqlDataReader reader;
            SqlCommand save = new SqlCommand();
            save.Connection = SQLConnection.con;
            save.Connection.Open();
            save.CommandType = System.Data.CommandType.StoredProcedure;
            save.CommandText = "IMS_DELETE_ATTRIBUTE";
            save.Parameters.AddWithValue("inGUID", property.m_GUID);
            reader = save.ExecuteReader();
            reader.Close();
            save.Connection.Close();
        }
    }
    public class StringListConverter : TypeConverter
    {
         ICollection myCol;

         public static string s0 = "Атрибут может содержать одно значение";
         public static string s1 = "Атрибут может содержать множество значений";
         public static string s2 = "Атрибут может содержать одно значение из списка разрешённых значений+";
         public static string s3 = "Атрибут может содержать множество значений из списка";

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true; // display drop
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true; // drop-down vs combo
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            // note you can also look at context etc to build list
            myCol = new string[] { s0, s1, s2, s3 };
            return new StandardValuesCollection(myCol);
        }


        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (string item in myCol)
                {
                    if (item == (string)value)
                    {
                        return item;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}