﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public Dictionary<string, Product> inventory = new Dictionary<string, Product>();

        public VendingMachine()
        {
            List<Product> products = new List<Product>();            

            using (StreamReader sr = new StreamReader("input.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] separated = line.Split("|");


                    Product product = new Product(separated[0], separated[1], decimal.Parse(separated[2]), separated[3]);
                    products.Add(product);
                }
            }

            foreach (Product item in products)
            {
                inventory.Add(item.Location, item);
            }
        }

        public void DisplayItems()
        {
            foreach (KeyValuePair<string, Product> kvp in inventory)
            {
                if (kvp.Value.Quantity > 0)
                {
                    Console.WriteLine($"> {kvp.Value.Location} | {kvp.Value.Name} | ${kvp.Value.Price} | {kvp.Value.Quantity} in stock");
                }
                else
                {
                    Console.WriteLine($"> {kvp.Value.Location} | {kvp.Value.Name} | ${kvp.Value.Price} | SOLD OUT"); 
                }
            }
        }

        public void DispenseProduct(string input)
        {            
            if (inventory.ContainsKey(input))
            {
                Product product = inventory[input];

                if (product.Quantity > 0)
                {
                    product.Quantity--;

                    switch (product.Type)
                    {
                        case "Chip":
                            Console.WriteLine("\n>>CRUNCH CRUNCH, YUM!<<");
                            break;

                        case "Candy":
                            Console.WriteLine("\n>>MUNCH MUNCH, YUM!<<");
                            break;

                        case "Drink":
                            Console.WriteLine("\n>>GLUG GLUG, YUM!<<");
                            break;

                        case "Gum":
                            Console.WriteLine("\n>>CHEW CHEW, YUM!<<");
                            break;
                    }                    
                }

                else
                {
                    Console.WriteLine("> Sorry! That item is out of stock.");
                }
            }

            else
            {
                Console.WriteLine("> Invalid Entry");
            }
        }
    }
}
