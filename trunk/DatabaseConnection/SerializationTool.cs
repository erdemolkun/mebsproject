using System;
using System.Collections.Generic;

using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace DatabaseConnection
{
    internal class SerializationTool<T> where T : class
    {
        public bool SerializeObject(string filename, T objectToSerialize)
        {
            try
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(T));
                TextWriter WriteFileStream = new StreamWriter(filename);
                SerializerObj.Serialize(WriteFileStream, objectToSerialize);
                WriteFileStream.Close();

                return true;
                /*Stream stream = File.Open(filename, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
                stream.Close();
                return true;*/
            }
            catch (Exception)
            {
                return false;
            }
        }
        public T DeSerializeObject(string filename)
        {
            FileStream ReadFileStream = null;
            try
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(T));

                // Create a new file stream for reading the XML file
                ReadFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Load the object saved above by using the Deserialize function
                T LoadedObj = (T)SerializerObj.Deserialize(ReadFileStream);
                
                return LoadedObj;
            }
            catch (Exception) { return null; }
            finally { if (ReadFileStream != null)ReadFileStream.Close(); }
        }


        public static string SerializeObjectToXML(object item)
        {
            try
            {
                string xmlText;
                Type objectType = item.GetType();
                XmlSerializer xmlSerializer = new XmlSerializer(objectType);
                MemoryStream memoryStream = new MemoryStream();
                using (XmlTextWriter xmlTextWriter =
                    new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    xmlSerializer.Serialize(xmlTextWriter, item);
                    memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                    xmlText = new UTF8Encoding().GetString(memoryStream.ToArray());
                    memoryStream.Dispose();
                    return xmlText;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.ToString());
                return null;
            }
        }
    }
}
