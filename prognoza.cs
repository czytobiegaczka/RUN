using System;
using System.Collections.Generic;

namespace RUN
{
    public class prognoza
    {
        private List<string> czas_od;
        public List<string> czas_do;
        public List<string> ikona;
        public List<string> opis;
        public List<string> opady;
        public List<string> wiatr;
        public List<string> temperatura;
        public List<string> cisnienie;
        public List<string> wilgotnosc;



        public prognoza()
        {
            czas_od = new List<string>();
            czas_do = new List<string>();
            ikona = new List<string>();
            opis = new List<string>();
            opady= new List<string>();
            wiatr = new List<string>();
            temperatura = new List<string>();
            cisnienie = new List<string>();
            wilgotnosc = new List<string>();
        }
        
        public void add_czas_od(string _czas_od)
        {
            czas_od.Add(_czas_od);   
        }
        public void add_czas_do(string _czas_do)
        {
            czas_do.Add(_czas_do);
        }
        public void add_ikona(string _ikona)
        {
            ikona.Add(_ikona);
        }
        public void add_opis(string _opis)
        {
            opis.Add(_opis);
        }
        public void add_opady(string _opady)
        {
            opady.Add(_opady);
        }
        public void add_wiatr(string _wiatr)
        {
            wiatr.Add(_wiatr);
        }
        public void add_temperatura(string _temperatura)
        {
            temperatura.Add(_temperatura);
        }
        public void add_cisnienie(string _temperatura)
        {
            cisnienie.Add(_temperatura);
        }
        public void add_wilgotnosc(string _wilgotnosc)
        {
            wilgotnosc.Add(_wilgotnosc);
        }

        public List<string> WeatherNow(int godzina)
        {
            int i = 0;
            Console.WriteLine("od: " + Convert.ToInt16(czas_od[i].Substring(czas_od[i].Length - 8, 2)));
            Console.WriteLine("do: " + Convert.ToInt16(czas_do[i].Substring(czas_do[i].Length - 8, 2)));
            Console.WriteLine("godz: " + godzina);

            if (godzina<=21)
            {
                while (Convert.ToInt16(czas_od[i].Substring(czas_od[i].Length - 8, 2)) <= godzina && Convert.ToInt16(czas_do[i].Substring(czas_do[i].Length - 8, 2)) < godzina)
                {
                    Console.WriteLine("od: " + Convert.ToInt16(czas_od[i].Substring(czas_od[i].Length - 8, 2)));
                    Console.WriteLine("do: " + Convert.ToInt16(czas_do[i].Substring(czas_do[i].Length - 8, 2)));
                    Console.WriteLine("godz: " + godzina);
                    i++;
                }
            }
            else
            {
                while (Convert.ToInt16(czas_od[i].Substring(czas_od[i].Length - 8, 2)) == 21 && Convert.ToInt16(czas_do[i].Substring(czas_do[i].Length - 8, 2)) == 0)
                {
                    Console.WriteLine("od: " + Convert.ToInt16(czas_od[i].Substring(czas_od[i].Length - 8, 2)));
                    Console.WriteLine("do: " + Convert.ToInt16(czas_do[i].Substring(czas_do[i].Length - 8, 2)));
                    Console.WriteLine("godz: " + godzina);
                    i++;
                }
            }



            List<string> today = new List<string>
            {
                czas_od[i],
                czas_do[i],
                ikona[i],
                opis[i],
                opady[i],
                wiatr[i],
                temperatura[i],
                cisnienie[i],
                wilgotnosc[i]
            };

            Console.WriteLine("temp: " + temperatura[i]);

            return today;
        }
        
    }
}