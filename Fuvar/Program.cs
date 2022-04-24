using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fuvar
{
    class Fuvar
    {
        int taxiAzonositoja;
        DateTime indulasIdopontja;
        int utazasIdotartama;
        double megtettTavolsag;
        double viteldij;
        double borravalo;
        string fizetesModja;

        public Fuvar(int taxiAzonositoja, DateTime indulasIdopontja, int utazasIdotartama, double megtettTavolsag, double viteldij, double borravalo, string fizetesModja)
        {
            TaxiAzonositoja = taxiAzonositoja;
            IndulasIdopontja = indulasIdopontja;
            UtazasIdotartama = utazasIdotartama;
            MegtettTavolsag = megtettTavolsag;
            Viteldij = viteldij;
            Borravalo = borravalo;
            FizetesModja = fizetesModja;
        }

        public int TaxiAzonositoja { get => taxiAzonositoja; set => taxiAzonositoja = value; }
        public DateTime IndulasIdopontja { get => indulasIdopontja; set => indulasIdopontja = value; }
        public int UtazasIdotartama { get => utazasIdotartama; set => utazasIdotartama = value; }
        public double MegtettTavolsag { get => megtettTavolsag; set => megtettTavolsag = value; }
        public double Viteldij { get => viteldij; set => viteldij = value; }
        public double Borravalo { get => borravalo; set => borravalo = value; }
        public string FizetesModja { get => fizetesModja; set => fizetesModja = value; }

        public override string ToString()
        {
            return $"{TaxiAzonositoja};{IndulasIdopontja};{UtazasIdotartama};{MegtettTavolsag};{Viteldij};{Borravalo};{FizetesModja}";
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            List<Fuvar> list = new List<Fuvar>(File.ReadAllLines("fuvar.csv",Encoding.UTF8).Skip(1).Count());

            //2.
            foreach (string item in File.ReadAllLines("fuvar.csv", Encoding.UTF8).Skip(1)) 
            {
                string[] oneLine = item.Split(';');
                list.Add(new Fuvar(int.Parse(oneLine[0]), DateTime.Parse(oneLine[1]), int.Parse(oneLine[2]), double.Parse(oneLine[3]), double.Parse(oneLine[4]), double.Parse(oneLine[5]), oneLine[6]));
            }

            //3.
            Console.WriteLine("3. feladat: {0} fuvar",list.Count);

            //4.
            uint fuvarokMennyisege = 0;
            double bevetel = 0;
            foreach (Fuvar item in list)
            {
                if (item.TaxiAzonositoja == 6185)
                {
                    fuvarokMennyisege = fuvarokMennyisege + 1;
                    bevetel = bevetel + item.Viteldij;
                }
            }

            Console.WriteLine("4. feladat: {0} fuvar alatt: {1}$",fuvarokMennyisege,bevetel);

            //5.
            HashSet<string> fizetesiModok = new HashSet<string>();
            foreach (Fuvar item in list)
            {
                fizetesiModok.Add(item.FizetesModja);
            }
            int [] fizetesimodSzamlalo = new int [fizetesiModok.Count];
            int aktualisDarabszam = 0;

            for (int i = 0; i < fizetesimodSzamlalo.Length; i++)
            {
                foreach  (Fuvar item in list)
                {
                    if (item.FizetesModja == fizetesiModok.ElementAt(i))
                    {
                        aktualisDarabszam = aktualisDarabszam + 1;
                    }

                }
                fizetesimodSzamlalo[i] = aktualisDarabszam;
                aktualisDarabszam = 0;
            }
            Console.WriteLine("5. feladat:");
            for (int i = 0; i < fizetesiModok.Count; i++)
            {
                Console.WriteLine("\t{0}: {1} fuvar",fizetesiModok.ElementAt(i),fizetesimodSzamlalo[i]);
            }


            //6.
            double osszfuvar = 0;
            foreach (Fuvar item in list)
            {
                osszfuvar = osszfuvar + item.MegtettTavolsag;
            }
            Console.WriteLine("6. feladat: {0}km",(osszfuvar*1.6).ToString("N2"));

            //7.
            double max = 0;
            foreach (Fuvar item in list)
            {
                if (item.UtazasIdotartama > max )
                {
                    max = item.UtazasIdotartama;
                    
                }
            }
            Console.WriteLine("7. feladat: Leghosszabb fuvar:");
            foreach (Fuvar item in list)
            {
                if (max == item.UtazasIdotartama)
                {
                    Console.WriteLine("\tFuvar hossza: {0} másodperc\n\tTaxi azonosito: {1}\n\tMegtett távolság: {2} km\n\tViteldíj: {3}$",item.UtazasIdotartama,item.TaxiAzonositoja,item.MegtettTavolsag,item.Viteldij);
                }
            }

            //8.

            List<Fuvar> kivalogatott = new List<Fuvar>();

            foreach (Fuvar item in list)
            {
                if (item.UtazasIdotartama > 0 && item.Viteldij > 0 && item.MegtettTavolsag ==0)
                {
                    kivalogatott.Add(item);
                }
            }
            kivalogatott = kivalogatott.OrderBy(x => x.IndulasIdopontja).ToList();

            FileStream ujfajl = new FileStream("hibak.txt", FileMode.Create,FileAccess.Write);
            StreamWriter iras = new StreamWriter(ujfajl, Encoding.UTF8);
            var elsosor = File.ReadAllLines("fuvar.csv", Encoding.UTF8);
            iras.WriteLine(elsosor.ElementAt(0).ToString());
            foreach (Fuvar item in kivalogatott)
            {
                
                iras.WriteLine($"{item.TaxiAzonositoja};{item.IndulasIdopontja};{item.UtazasIdotartama};{item.MegtettTavolsag};{item.Viteldij};{item.Borravalo};{item.FizetesModja}");

            }
            iras.Close();
            ujfajl.Close();
            Console.WriteLine("8. feladat: hibak.txt");

            Console.ReadKey();
        }
    }
}