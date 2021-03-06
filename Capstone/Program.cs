﻿using Capstone.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // Will Track Total Profit of Vending Machine for Sales Report
            decimal totalProfit = 0;

            decimal balance = 0;

            VendingMachine vendingMachine = new VendingMachine();

            while (true)
            {
                Console.WriteLine("\n**Welcome to the Vend-O-Matic 4000**\n\nSelect Option Below\n-------------------");
                Console.WriteLine("(1) View Inventory \n(2) Select Product \n(3) Quit\n");
                Console.Write($"> Current Balance: ${balance} \n\n>>:");
                string menu = Console.ReadLine();
                Console.WriteLine("");

                if (menu == "1")
                {
                    vendingMachine.DisplayItems();
                }
                else if (menu == "2")
                {
                    bool valid = false;

                    do
                    {
                        Console.Write(">> Please enter the location of the product you'd like to puchase: ");

                        string input = Console.ReadLine().ToUpper();

                        valid = false;

                        if (vendingMachine.inventory.ContainsKey(input))
                        {
                            Product product = vendingMachine.inventory[input];

                            if (product.Quantity > 0)
                            {
                                Console.Write($"\n> Price of item selected : ${product.Price} \n\n> {product.Quantity} Remaining\n");

                                bool run = true;

                                while (run)
                                {
                                    Console.Write($"\n> Current Balance is ${balance}\n\n>> Please Enter a Whole Dollar Amount(1, 2, 5, 10), (d) to dispence or (s) to start over\n\n>>: ");
                                    string input2 = Console.ReadLine();

                                    if (input2 == "1" || input2 == "2" || input2 == "5" || input2 == "10")
                                    {
                                        int amount = int.Parse(input2);
                                        balance += (decimal)amount;

                                        Console.WriteLine($"> ${input2} inserted.");

                                        using (StreamWriter sw = new StreamWriter("log.txt", true))
                                        {
                                            sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")} FEED MONEY: ${amount}     ${balance}");
                                        }
                                    }
                                    else if (input2 == "D" || input2 == "d")
                                    {
                                        if (balance >= product.Price)
                                        {
                                            run = false;
                                            vendingMachine.DispenseProduct(input);
                                            using (StreamWriter sw = new StreamWriter("log.txt", true))
                                            {
                                                sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")} {product.Name} {product.Location} ${balance}     ${balance - product.Price}");
                                            };
                                            balance -= product.Price;
                                            totalProfit += product.Price;
                                        }
                                        else
                                        {
                                            Console.WriteLine("> Insufficient Funds, please add more money.");
                                        }
                                    }
                                    else if (input2 == "S" || input2 == "s")
                                    {
                                        Console.WriteLine("");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("> Invalid Entry. Please enter valid input.");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("\n>>Sorry that item is SOLD OUT<<\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n> Invalid Entry. Please enter valid input.\n");
                            valid = true;
                        }
                    } while (valid);
                }
                else if (menu == "3")
                {
                    Console.WriteLine("> Thank You for using Vend-O-Matic 4000!");
                    Console.WriteLine($"> Your remaining balance is: ${balance}");

                    using (StreamWriter sw = new StreamWriter("log.txt", true))
                    {
                        sw.WriteLine($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")} GIVE CHANGE: ${balance}     $0.00");
                    };
                    int numQuarter = 0;
                    int numDime = 0;
                    int numNickel = 0;

                    while (balance > 0)
                    {
                        if (balance >= 0.25M)
                        {
                            numQuarter++;
                            balance -= 0.25M;
                        }
                        else if (balance >= 0.10M)
                        {
                            numDime++;
                            balance -= 0.10M;
                        }
                        else if (balance >= 0.50M)
                        {
                            numNickel++;
                            balance -= 0.05M;
                        }
                    }

                    Console.WriteLine($"> Here is your change: {numQuarter} Quarter(s), {numDime} Dime(s), {numNickel} Nickel(s).\n");
                    Console.WriteLine("\n------------------------------------------------------------------------------\n");
                }
                else if (menu == "4")
                {
                    foreach (KeyValuePair<string, Product> kvp in vendingMachine.inventory)
                    {
                        Console.WriteLine($"{kvp.Value.Name} | {kvp.Value.Sold}");
                    }

                    Console.WriteLine($"\n\n**TOTAL SALES** ${totalProfit}\n");
                }
                else
                {
                    Console.WriteLine("\n> Invalid Entry. Please enter valid input.\n");
                }
            }
        }
    }
}
