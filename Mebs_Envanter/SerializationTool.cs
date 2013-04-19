using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Mebs_Envanter
{
    public class SerializationTool<T> where T : class
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
            try
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(T));

                // Create a new file stream for reading the XML file
                FileStream ReadFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

                // Load the object saved above by using the Deserialize function
                T LoadedObj = (T)SerializerObj.Deserialize(ReadFileStream);

                // Cleanup
                ReadFileStream.Close();
                /*
                DevicePersistantList objectToSerialize;
                Stream stream = File.Open(filename, FileMode.Open);
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (DevicePersistantList)bFormatter.Deserialize(stream);
                stream.Close();
                return objectToSerialize;*/

                return LoadedObj;
            }
            catch (Exception) { return null; }
        }
        //public class DeviceSerializer<T> where T : class
        //{
        //    public DeviceSerializer() { }
        //    public bool SerializeObject(string filename, T objectToSerialize)
        //    {
        //        try
        //        {
        //            Stream stream = File.Open(filename, FileMode.Create);
        //            BinaryFormatter bFormatter = new BinaryFormatter();
        //            bFormatter.Serialize(stream, objectToSerialize);
        //            stream.Close();
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }

        //    public T DeSerializeObject(string filename)
        //    {
        //        try
        //        {
        //            T objectToSerialize;
        //            Stream stream = File.Open(filename, FileMode.Open);
        //            BinaryFormatter bFormatter = new BinaryFormatter();
        //            objectToSerialize = (T)bFormatter.Deserialize(stream);
        //            stream.Close();
        //            return objectToSerialize;
        //        }
        //        catch (Exception) { return null; }
        //    }
        //}
    }
}
