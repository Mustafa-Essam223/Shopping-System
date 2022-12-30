using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShoppingSystem
{
    [Serializable]
    class items
    {
        public int item_ID;
        public string item_Name;
        public int items_Quantity;
        public items(string item_Name, int item_ID, int items_Quantity)
        {
            this.item_Name = item_Name;
            this.item_ID = item_ID;
            this.items_Quantity = items_Quantity;
        }
        public void Display()
        {
            Console.WriteLine("item's name : " + item_Name);
            Console.WriteLine("item's ID : " + item_ID);
            Console.WriteLine("item's Quantity : " + items_Quantity);
        }
    }
    class ShoppingCart
    {
        public int User_ID;
        public Dictionary<int, items> itemsList;
        BinaryFormatter formatter;
        string fileName;


        public ShoppingCart(string fileName)
        {
            this.fileName = fileName;
            //FileStream stream = new FileStream(fileName, FileMode.Create);

            formatter = new BinaryFormatter();
            itemsList = new Dictionary<int, items>();
            load();
        }
        public void load()
        {
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate);
            while (stream.Position != stream.Length)
            {
                Dictionary<int, items> temp =
                    (Dictionary<int, items>)formatter.Deserialize(stream);
                for (int i = 0; i < temp.Count; i++)
                    itemsList.Add(temp.ElementAt(i).Key, temp.ElementAt(i).Value);
            }

            stream.Close();
        }
        public void Print()
        {
            bool f = false;
            for (int i = 0; i < itemsList.Count; i++)
            {
                Console.Write("User ID:" + itemsList.ElementAt(i).Key + "\n");
                itemsList.ElementAt(i).Value.Display();
                f = true;
            }

            if (!f) Console.WriteLine("Cart is Empty");
        }
        public void Add(int UserID, items item)
        {
            this.User_ID = UserID;
            if (!itemsList.ContainsKey(User_ID) && !itemsList.ContainsValue(item))
            {
                itemsList.Add(User_ID, item);
            }

            else
            {
                Console.WriteLine("This item is already exists !!");
            }
        }
        public bool remove(int itemIDD)
        {
            for (int i = 0; i < itemsList.Count; i++)
            {
                if (itemsList.ElementAt(i).Value.item_ID.Equals(itemIDD))
                {
                    itemsList.Remove(itemsList.ElementAt(i).Key);
                    return true;
                }

            }
            return false;
        }
        public void save()
        {
            FileStream stream = new FileStream(fileName, FileMode.Append);
            formatter.Serialize(stream, itemsList);
            stream.Close();
        }
        public void Search(int ID)
        {
            bool f = false;
            for (int i = 0; i < itemsList.Count; i++)
            {
                if (itemsList.ElementAt(i).Key.Equals(ID))
                {
                    f = true;
                    Console.WriteLine("User items : ");
                    itemsList.ElementAt(i).Value.Display();
                    break;
                }
            }


            if (!f) Console.WriteLine("User isn't in the system");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            ShoppingCart cart = new ShoppingCart("Carts.txt");
            while (true)
            {

                //Console.WriteLine("Please Enter Add to add new items to the cart "
                //+ " or Display to display the items or Search By ID ");
                Console.WriteLine("Enter character of your choice:\n" +
                    "1- [A or a]  add new item to the cart\n" +
                    "2- [D or d]  display items\n" +
                    "3- [S or s]  Search for a specific user");

                string cmnd = Console.ReadLine();
                if (cmnd[0]=='A' || cmnd[0] == 'a')
                {
                    while (true)
                    {

                        Console.WriteLine("Enter Item id :");
                        int item_id = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter item Name :");
                        string name = Console.ReadLine();


                        Console.WriteLine("Enter Item Quantity :");
                        int Quantity = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter User id :");
                        int User_id = int.Parse(Console.ReadLine());
                        items item = new items(name, item_id, Quantity);
                        cart.Add(User_id, item);

                        Console.WriteLine("Would you like to buy more items? Y|N");
                        char c = char.Parse(Console.ReadLine());
                        if (c.Equals('N'))
                        {

                            Console.WriteLine("Would You Like to remove newely " +
                                "added items before Saving ?Y|N");
                            char response = char.Parse(Console.ReadLine());
                            if (response.Equals('N'))
                            {
                                cart.save();
                                cart.itemsList.Clear();
                            }
                            else if (response.Equals('Y'))
                            {
                                Console.WriteLine("Please Enter The id of item you want to delete");
                                int id = int.Parse(Console.ReadLine());
                                if (cart.remove(id)) Console.WriteLine("Done");
                                else Console.WriteLine("Unvalid ID!!");
                                Console.WriteLine("Remove More ? Y|N");
                                char res = char.Parse(Console.ReadLine());
                                if (res.Equals('N'))
                                {
                                    cart.save();
                                    cart.itemsList.Clear();
                                    break;
                                }

                            }
                            break;
                        }
                        else if (c.Equals('Y'))
                            continue;
                    }
                }
                else if (cmnd[0] == 'D' || cmnd[0] == 'd')
                {
                    cart.load();
                    cart.Print();
                }
                else if (cmnd[0] == 'S' || cmnd[0] == 's')
                {
                    //cart.load();
                    Console.WriteLine("Enter User ID :");
                    cart.Search(int.Parse(Console.ReadLine()));
                }
                else 
                    Console.WriteLine("Unvalid Cmnd!!") ;
            }
        }
    }
}
