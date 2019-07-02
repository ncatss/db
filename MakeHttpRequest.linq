<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	start();
	
//	var trade = new Trade();
//	trade.Add("1","T0001","ConA", 1, 1, DateTime.Today);
//	trade.Add("2","T0001","ConB", 1, 1, DateTime.Today.AddDays(-1));
//	trade.Add("3","T0001","ConB", 1, 1, DateTime.Today);
//	trade.Add("4","T0001","ConA", 1, 1, DateTime.Today);
//	trade.Add("5","T0001","ConB", 1, 1, DateTime.Today);
//	trade.Add("6","T0002","ConA", 1, 1, DateTime.Today.AddDays(-1));
//	trade.Add("7","T0002","ConB", 1, 1, DateTime.Today);
//	trade.Add("8","T0002","ConA", 1, 1, DateTime.Today);
//	trade.Add("9","T0002","ConB", 1, 1, DateTime.Today.AddDays(-1));
//	trade.Add("10","T0002","ConA", 1, 1, DateTime.Today);
//	
//	trade.GetAll();
//	trade.GetAllByCon("ConA");
//	trade.GetAllByAcct("T0001");
//	trade.GetAllByDate(DateTime.Today);
//	
//	trade.Get("1");
//	trade.Edit("1", timeInUTC: DateTime.Today.AddDays(-3));
//	trade.Delete("10");
//	trade.CheckExit("3");
//	trade.GetAll();
	
//	var trader = new Trader();
//	trader.GetAll();
//	trader.Add("0","yn","1234","y@gmail.com");
//	trader.Add("1","ny","3456","n@gmail.com");
//	trader.Add("2","yt","2345","ty@gmail.com");
//	trader.GetAll();
//	trader.Get("yn");
//	trader.Edit("yn", phoneNum: "13567");
//	trader.Delete("yt");
//	trader.CheckExit("yt");
//	trader.GetAll();
	
//	var strat = new Strat();
//	strat.GetAll();
//	strat.Add("0","strat1", "yn");
//	strat.Add("1","strat2", "yn");
//	strat.Add("2","strat3", "ny");
//	strat.GetAll();
//	strat.GetAll("yn");
//	strat.Get("0");
//	strat.Delete("1");
	
//	var acct = new Acct();
//	acct.GetAll();
//	acct.Add("T0001","yn test","yn");
//	acct.Add("T0002","yn2 test","yn");
//	acct.Add("T0003","ny test","ny");
//	acct.Get("T0003");
//	acct.Edit("T0003", acctDes: "yn test");
//	acct.GetAll("yn");	
//	acct.CheckExit("T0001");
//	acct.Delete("T0003");
//	acct.GetAll("yn");
}

public void start()
{
	var myProcess = new System.Diagnostics.ProcessStartInfo();
	myProcess.UseShellExecute = true;
	myProcess.CreateNoWindow = false;
	myProcess.FileName = "java";
	myProcess.Arguments = "-jar C:\\Users\\yini\\Desktop\\Files\\RestAPI\\db\\target\\dbtest-1.0.1.jar";
	Process.Start(myProcess);
}

public class Acct
{
	public String acctNum {get;set;}
	public String acctDes {get;set;}
	public String reportTo {get;set;} // to trader
	
	private string baseUri = "http://localhost:4567/api/accts";
	
	public void Add(String acctNum, String acctDes, String reportTo)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new {acctDes = acctDes, acctNum = acctNum, reportTo = reportTo}), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PostURL(new Uri(baseUri), c));
		Response.waitResponse(t, "POST");
	}

	public void GetAll()
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri)));
		Response.waitResponse(t, "GET");
	}

	public void GetAll(String traderName)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"?traderName={traderName}")));
		Response.waitResponse(t, "GETByTrader");
	}

	public void Get(String acctNum)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"/{acctNum}")));
		Response.waitResponse(t, "GETByAcct");
	}

	public void Edit(String acctNum, String acctDes = null, String reportTo = null)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { acctDes = acctDes, reportTo = reportTo }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PutURL(new Uri(baseUri + $"/{acctNum}"), c));
		Response.waitResponse(t, "PUT");
	}

	public void Delete(String acctNum)
	{
		var t = Task.Run(() => DeleteURL(new Uri(baseUri + $"/{acctNum}")));
		Response.waitResponse(t, "DELETE");
	}

	public void CheckExit(String acctNum)
	{
		var t = Task.Run(() => OptionsURL(new Uri(baseUri + $"/{acctNum}")));
		Response.waitResponse(t, "OPTIONS");
	}
	
	private class Response
	{
		public string reqType;
		public string status { get; set; }
		public string message { get; set; }
		[JsonConverter(typeof(SingleValueArrayConverter<Acct>))]
		public List<Acct> data { get; set; }

		public static void waitResponse(Task<String> t, string type)
		{
			t.Wait();
			//t.Result.Dump();
			try
			{
				var res = JsonConvert.DeserializeObject<Response>(t.Result);
				res.reqType = type;
				res.Dump();
			}
			catch { }

			Console.ReadLine();
		}
		public object ToDump()
		{
			IDictionary<string, object> custom = new System.Dynamic.ExpandoObject();
			custom["reqType"] = reqType;
			if (status != null)
				custom["status"] = status;
			if (message != null)
				custom["msg"] = message;
			if (data != null)
				custom["accts"] = data;
			return custom;
		}
	}
}

public class Strat
{
	public String id {get;set;}
	public String stratName {get;set;}
	public String createBy {get;set;}

	private string baseUri = "http://localhost:4567/api/strats";
	public void Add(String id, String stratName, String createBy)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { id = id, stratName = stratName, createBy = createBy }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PostURL(new Uri(baseUri), c));
		Response.waitResponse(t, "POST");
	}
	public void GetAll()
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri)));
		Response.waitResponse(t, "GET");
	}

	public void GetAll(String traderName)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"?traderName={traderName}")));
		Response.waitResponse(t, "GETByTrader");
	}
	public void Get(String id)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"/{id}")));
		Response.waitResponse(t, "GETById");
	}
	public void Delete(String id)
	{
		var t = Task.Run(() => DeleteURL(new Uri(baseUri + $"/{id}")));
		Response.waitResponse(t, "DELETE");
	}
	
	private class Response
	{
		public string reqType;
		public string status { get; set; }
		public string message { get; set; }
		[JsonConverter(typeof(SingleValueArrayConverter<Strat>))]
		public List<Strat> data { get; set; }

		public static void waitResponse(Task<String> t, string type)
		{
			t.Wait();
			//t.Result.Dump();
			try
			{
				var res = JsonConvert.DeserializeObject<Response>(t.Result);
				res.reqType = type;
				res.Dump();
			}
			catch { }

			Console.ReadLine();
		}
		public object ToDump()
		{
			IDictionary<string, object> custom = new System.Dynamic.ExpandoObject();
			custom["reqType"] = reqType;
			if (status != null)
				custom["status"] = status;
			if (message != null)
				custom["msg"] = message;
			if (data != null)
				custom["strats"] = data;
			return custom;
		}
	}
}

public class Trader
{
	public String id {get;set;}
	public String name {get;set;}
	public String phoneNum {get;set;}
	public String email {get;set;}

	private string baseUri = "http://localhost:4567/api/traders";
	public void Add(String id, String name, String phoneNum, String email)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { id = id, name = name, phoneNum = phoneNum, email = email }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PostURL(new Uri(baseUri), c));
		Response.waitResponse(t, "POST");
	}
	public void GetAll()
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri)));
		Response.waitResponse(t, "GET");
	}
	public void Get(String name)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"/{name}")));
		Response.waitResponse(t, "GETByName");
	}

	public void Edit(String name, String id = null, String phoneNum = null, String email = null)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { id = id, phoneNum = phoneNum, email = email }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PutURL(new Uri(baseUri + $"/{name}"), c));
		Response.waitResponse(t, "PUT");
	}

	public void Delete(String name)
	{
		var t = Task.Run(() => DeleteURL(new Uri(baseUri + $"/{name}")));
		Response.waitResponse(t, "DELETE");
	}

	public void CheckExit(String name)
	{
		var t = Task.Run(() => OptionsURL(new Uri(baseUri + $"/{name}")));
		Response.waitResponse(t, "OPTIONS");
	}

	private class Response
	{
		public string reqType;
		public string status { get; set; }
		public string message { get; set; }
		[JsonConverter(typeof(SingleValueArrayConverter<Trader>))]
		public List<Trader> data { get; set; }

		public static void waitResponse(Task<String> t, string type)
		{
			t.Wait();
			//t.Result.Dump();
			try
			{
				var res = JsonConvert.DeserializeObject<Response>(t.Result);
				res.reqType = type;
				res.Dump();
			}
			catch { }

			Console.ReadLine();
		}
		public object ToDump()
		{
			IDictionary<string, object> custom = new System.Dynamic.ExpandoObject();
			custom["reqType"] = reqType;
			if (status != null)
				custom["status"] = status;
			if (message != null)
				custom["msg"] = message;
			if (data != null)
				custom["traders"] = data;
			return custom;
		}
	}
}

public class Trade
{
	public String id {get;set;}
	public String tradeAcct {get;set;}
	public String contract {get;set;}
	public int price {get;set;}
	public int qty {get;set;}
	public DateTime timeInUTC {get;set;}

	private string baseUri = "http://localhost:4567/api/trades";
	public void Add(String id, String tradeAcct, String contract, int price, int qty, DateTime timeInUTC)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { id = id, tradeAcct = tradeAcct, contract = contract, price = price, qty = qty, timeInUTC = $"{timeInUTC:yyyy-MM-dd}" }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PostURL(new Uri(baseUri), c));
		Response.waitResponse(t, "POST");
	}
	public void GetAll()
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri)));
		Response.waitResponse(t, "GET");
	}
	public void GetAllByCon(String con)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"?con={con}")));
		Response.waitResponse(t, "GETByCon");
	}
	public void GetAllByAcct(String acct)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"?acct={acct}")));
		Response.waitResponse(t, "GETByAcct");
	}
	public void GetAllByDate(DateTime timeInUTC)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"?date={timeInUTC:yyyy-MM-dd}")));
		Response.waitResponse(t, "GETByDate");
	}
	public void Get(String id)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri + $"/{id}")));
		Response.waitResponse(t, "GETById");
	}

	public void Edit(String id, String tradeAcct = null, String contract = null, int? price = null, int? qty = null, DateTime? timeInUTC = null)
	{
		HttpContent c = new StringContent(JsonConvert.SerializeObject(new { tradeAcct = tradeAcct, contract = contract, price = price, qty = qty, timeInUTC = timeInUTC.HasValue ? $"{timeInUTC:yyyy-MM-dd}" : null }), Encoding.UTF8, "application/json");
		var t = Task.Run(() => PutURL(new Uri(baseUri + $"/{id}"), c));
		Response.waitResponse(t, "PUT");
	}

	public void Delete(String id)
	{
		var t = Task.Run(() => DeleteURL(new Uri(baseUri + $"/{id}")));
		Response.waitResponse(t, "DELETE");
	}

	public void CheckExit(String id)
	{
		var t = Task.Run(() => OptionsURL(new Uri(baseUri + $"/{id}")));
		Response.waitResponse(t, "OPTIONS");
	}

	private class Response
	{
		public string reqType;
		public string status { get; set; }
		public string message { get; set; }
		[JsonConverter(typeof(SingleValueArrayConverter<Trade>))]
		public List<Trade> data { get; set; }

		public static void waitResponse(Task<String> t, string type)
		{
			t.Wait();
			//t.Result.Dump();
			try
			{
				var res = JsonConvert.DeserializeObject<Response>(t.Result);
				res.reqType = type;
				res.Dump();
			}
			catch { }

			Console.ReadLine();
		}
		public object ToDump()
		{
			IDictionary<string, object> custom = new System.Dynamic.ExpandoObject();
			custom["reqType"] = reqType;
			if (status != null)
				custom["status"] = status;
			if (message != null)
				custom["msg"] = message;
			if (data != null)
				custom["trades"] = data;
			return custom;
		}
	}
}

static async Task<string> GetURL(Uri u)
{
	
	var response = string.Empty;
	using (var client = new HttpClient())
	{
		HttpResponseMessage result = await client.GetAsync(u);
		if (result.IsSuccessStatusCode)
		{
			response = await result.Content.ReadAsStringAsync();
		}
	}
	return response;
}

static async Task<string> PostURL(Uri u, HttpContent c)
{
	var response = string.Empty;
	using (var client = new HttpClient())
	{
		HttpResponseMessage result = await client.PostAsync(u, c);
		if (result.IsSuccessStatusCode)
		{
			response = await result.Content.ReadAsStringAsync();
		}
	}
	return response;
}

static async Task<string> PutURL(Uri u, HttpContent c)
{
	var response = string.Empty;
	using (var client = new HttpClient())
	{
		HttpResponseMessage result = await client.PutAsync(u, c);
		if (result.IsSuccessStatusCode)
		{
			response = await result.Content.ReadAsStringAsync();
		}
	}
	return response;
}

static async Task<string> DeleteURL(Uri u)
{

	var response = string.Empty;
	using (var client = new HttpClient())
	{
		HttpResponseMessage result = await client.DeleteAsync(u);
		if (result.IsSuccessStatusCode)
		{
			response = await result.Content.ReadAsStringAsync();
		}
	}
	return response;
}

static async Task<string> OptionsURL(Uri u)
{

	var response = string.Empty;
	using (var client = new HttpClient())
	{
		var request = new HttpRequestMessage(HttpMethod.Options, u);
		HttpResponseMessage result = await client.SendAsync(request);
		if (result.IsSuccessStatusCode)
		{
			response = await result.Content.ReadAsStringAsync();
		}
	}
	return response;
}

public class SingleValueArrayConverter<T> : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		object retVal = new Object();
		if (reader.TokenType == JsonToken.StartObject)
		{
			T instance = (T)serializer.Deserialize(reader, typeof(T));
			retVal = new List<T>() { instance };
		}
		else if (reader.TokenType == JsonToken.StartArray)
		{
			retVal = serializer.Deserialize(reader, objectType);
		}
		return retVal;
	}

	public override bool CanConvert(Type objectType)
	{
		return true;
	}
}