﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson_1
{
    internal class Program
    { 
        
        static void Main(string[] args){

            /*
             
            BassCoin je moje virtualni kripto mena.
            1 bassCoin = <0.1:0.9> dollar.

            Formule logistickeho mapování Xn+1 = R*Xn *(1 - X n), je velmi dobra formule pro vytvaření chaotického grafu.
            Logistic map. pokud R bude kolem 3.75, je možny dostat rondomny čisla.
            */
            

            double bassKoint_soucastnaCena = 0.5; //X=<0:1>  
            double R = 3.75; //MESICNI KOFICENT
            double mojeBassCoin = 0;
            int akceZaDen = 116;
            Console.WriteLine("Binancia\nAhoj. Chces obchodovat s kryptoměnami?\nPokud Ano zmačkni \"A\", pokud ne \"N\"");
            char chciObchodovat = char.ToUpper(Console.ReadKey().KeyChar);
            double mamDolaru = 0;

            if (chciObchodovat == 'A')
            {
                Console.WriteLine("\nDobře.\n Kolik dollaru chcete investovat?");

                mamDolaru = interval(onlyNumberic(Console.ReadLine()),0,100000000); 
               
                Console.WriteLine("\nNa učtu mate {0} dollaru a {1} BassCoinu. \nPojdme programujme robota. " +
                    "\nZa kolik dollaru ma kupovat robot basscoin při snižení cene. " +
                    "\nPozor tato častka musi byt menši než častka co mate na učtu ({0}).", mamDolaru, mojeBassCoin);
                
                double investicniDollar = interval(onlyNumberic(Console.ReadLine()),0,mamDolaru);

                Console.WriteLine("\nUvedte procenta kolik basscoin musi prodat robot při zvýšené cene." +
                    "\n Pozor tato hodnota musi byt (0:1). Doporučujeme 0,2. ");
                
                double procent = interval(onlyNumberic(Console.ReadLine()),0,1);

                Console.WriteLine("\nUvedte chranice, kdy robot ma kupovat basscoin. " +
                    "\nHodnota musi byt (0:0,7). Doporučujeme uvest kolem 0,4.");

                double minimalniChranice = interval(onlyNumberic(Console.ReadLine()),0,7);
                
                Console.WriteLine("\nUvedte chranice, kdy robot ma prodavat basscoin. " +
                    "\nHodnota musi byt ({0}:0,9). Doporučujeme uvest kolem 0,6.",minimalniChranice);
                
                double maximalniChranice = interval(onlyNumberic(Console.ReadLine()),minimalniChranice,9);

                for (int mesic = 0; mesic < 30; mesic++)
                {
                    int[] sloupec = new int[akceZaDen];
                    double[] mapaBassCoinu = new double[akceZaDen];

                    //tady dostavame randomne čislo a ukladam je na pole.
                    for (int den = 0; den < akceZaDen; den++)
                    {
                        double tmp = 1 - bassKoint_soucastnaCena;
                        mapaBassCoinu[den] = bassKoint_soucastnaCena = R * bassKoint_soucastnaCena * tmp;
                        sloupec[den] = (int)(bassKoint_soucastnaCena * 10);
                    }

                    for (int i = 0; i < akceZaDen; i++)
                    {
                        int tranzakce = 3;
                        //Console.Write(mapaBassCoinu[i] + " ");
                        if (mapaBassCoinu[i] < minimalniChranice && mamDolaru > 0 && tranzakce > 0 && mamDolaru >= investicniDollar)
                        {
                            //tady kupujeme bassCoin
                            mamDolaru -= investicniDollar;
                            mojeBassCoin += dollarToBasscoin(investicniDollar, mapaBassCoinu[i]);
                            tranzakce--;
                        }
                        else if (mapaBassCoinu[i] > maximalniChranice && mojeBassCoin > 0)
                        {
                            //tady prodavame bassCoin
                            mamDolaru += (basscoinToDollar(mojeBassCoin, mapaBassCoinu[i]) * procent);
                            mojeBassCoin -= (mojeBassCoin * procent);
                            tranzakce++;

                        }

                    }

                    Console.WriteLine("\n");

                    //tady namalujeme graf
                    for (int row = 10; row > 0; row--)
                    {
                        for (int j = 0; j < akceZaDen; j++)
                        {
                            drawMap(sloupec[j], row, j);
                        }
                        Console.WriteLine("");
                    }
                    Console.WriteLine("\nNa učtu mate{0} dollaru a {1} BassCoinu. ", mamDolaru, mojeBassCoin);
                }
            }else if (chciObchodovat == 'N')
            {
                Console.WriteLine("\nDobře.\nPřeji pekni den");
            }
        }
        
        static void drawMap(int mapa, int row,int sloupec)
        {
            if (sloupec == 0) { Console.Write(row + ":"); }
            if (mapa == row) { Console.Write("X"); }
            else
            {
                Console.Write(".");

            }
        }
        static double dollarToBasscoin(double zaDollar, double s_CenaCoinu)
        {
            return zaDollar/ s_CenaCoinu; 
        }
        static double basscoinToDollar(double zaBasscoin, double s_CenaCoinu)
        {
            return zaBasscoin * s_CenaCoinu;
        }
        static double onlyNumberic(String tmp)
        {
            Boolean isTrue = true;
            double tmpDouble;
            while (isTrue) { 
                double number;
                if (double.TryParse(tmp, out number))
                {
                    isTrue = false;
                    return tmpDouble = double.Parse(tmp);
                }
                else
                {
                    isTrue = true;
                    Console.WriteLine("Zadejte prosim čiselnou hodnotu");
                    tmp = Console.ReadLine();
                }
            }
            return tmpDouble = double.Parse(tmp);  
        }
        static double interval(double number, double start, double finish)
        {
            Boolean isTrue = true ;
            while (isTrue)
            {
                if (number <= start)
                {
                    Console.WriteLine("Zadejte vetši čislo");
                   number = onlyNumberic(Console.ReadLine());
                    isTrue = true;

                } else if (number >= finish  ){ 
                    Console.WriteLine("Zadejte menši čislo");
                    number = onlyNumberic(Console.ReadLine());
                    isTrue = true;
                }else if (number > start && number < finish)
                {
                    isTrue = false;
                    Console.WriteLine("\nPrijato!");
                    return number;

                }
            }
            return number;
        }

    }

}
