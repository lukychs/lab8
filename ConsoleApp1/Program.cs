using System;
using System.Xml.Serialization;
using ClassLibrary10;

    
class Program
{
    static void Main(string[] args)
    {
        Cow cow = new Cow("Russia", true, "cow", "Cow");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Cow));

        // получаем поток, куда будем записывать сериализованный объект
        using (FileStream fs = new FileStream("cow.xml", FileMode.OpenOrCreate))
        {
            xmlSerializer.Serialize(fs, cow);
            Console.WriteLine("Объект был сериализован");
        }

        using (FileStream fs = new FileStream("cow.xml", FileMode.OpenOrCreate))
        {
            Cow? cow1 = xmlSerializer.Deserialize(fs) as Cow;
            Console.WriteLine($"Country: {cow1?.Country}\nHide: {cow1?.HideFromOtherAnimals}\nName: {cow1?.Name}\nType: {cow1?.WhatAnimal}");
        }
    }
}
