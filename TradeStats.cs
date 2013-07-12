using System;
using System.IO;
using System.Collections.Generic;

// compile like this :-
// C:\Program Files\Mono-2.10.9\bin>gmcs hello.cs
// run like this :-
// C:\Program Files\Mono-2.10.9\bin>mono hello.exe

public class InstrumentStat
{
  public string Symbol {get; set;}
	public int LastTimestamp {get; set;}
	public int Quantity {get; set;}
	public int Price {get;set;}
	public int MinPrice {get;set;}
	public int MaxPrice {get;set;}
	public int Gap {get;set;}
	public int Total {get;set;}
	public int TTS {get;set;}

	public string Summary()
	{
		return String.Join(",",
					new String[] {Symbol,Gap.ToString(),Total.ToString(),
							(TTS/Total).ToString(),MaxPrice.ToString()});
	}

	public void Update(int t, int q, int p)
	{
		if(p<MinPrice)
		{
			MinPrice = p;
		}

		if(p>MaxPrice)
		{
			MaxPrice = p;
		}

		Total += q;

		TTS += (q*p);

		int ng = t - LastTimestamp;

		if (ng > Gap)
		{
			Gap = ng;
		}

		LastTimestamp = t;
	}

	public InstrumentStat(string s,int lt, int q, int p)
	{
		Symbol = s;
		LastTimestamp = lt;
		Quantity = q;
		Price = p;
		MinPrice = p;
		MaxPrice = p;
		Gap = 0;
		Total = q;
		TTS = q * p;
	}
}
 
public class TradeStats
{
	static public void Main ()
	{
		int line_num = 1;
		string line;
		SortedDictionary<string, InstrumentStat> d = 
			new SortedDictionary<string, InstrumentStat>();

		System.IO.StreamReader file = 
    			new System.IO.StreamReader(@"input.csv");

		while((line = file.ReadLine()) != null)
		{
			string[] parts = line.Split(',');
			int t = Convert.ToInt32(parts[0]);
			string s = parts[1];
			int q = Convert.ToInt32(parts[2]);
			int p = Convert.ToInt32(parts[3]);

			if(!d.ContainsKey(s))
			{
			// Add the new record
			d.Add(s,new InstrumentStat(s,t,q,p));
			}
			else
			{
			// Update the existing record
			InstrumentStat i = d[s];
			i.Update(t,q,p);
			}

			line_num++;
		}

		file.Close();

		// Write out the sorted results
		TextWriter tw = new StreamWriter("output.csv");
		foreach (KeyValuePair<string, InstrumentStat> pair in d)
		{
			tw.WriteLine(pair.Value.Summary());
		}
		tw.Close();

	}
 
}
