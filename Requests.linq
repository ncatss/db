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
	var api = new DbRestAPI();
	api.GetALL(Type.Acct);
	api.AddAcct("T0001", "acct1", "yn");
	api.AddAcct("T0002", "acct2", "yn");
	api.AddAcct("T0003", "acct2", "yn");
	api.Delete(Type.Acct, "T0001");
	api.EditAcct("T0002", "acct test");
	api.Get(Type.Acct, "T0003");
	api.GetALLAcctByName("yn");
	api.Exist(Type.Acct, "T0001");
}

public class DbRestAPI{

	public DbRestAPI(){
		startServer();
	}
	// All API method calls
	public void GetALL(Type type)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(type))));
		waitResponse(t, $"Query all {type}:");
	}
	public void GetALLAcctByName(String traderName)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(Type.Acct) + $"?traderName={traderName}")));
		waitResponse(t, "GetALLAcctByName:");
	}
	public void GetALLStratByName(String traderName)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(Type.Strat) + $"?traderName={traderName}")));
		waitResponse(t, "GetALLStratByName:");
	}

	public void GetAllTradeByCon(String con)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(Type.Trade) + $"?con={con}")));
		waitResponse(t, "GetAllTradeByCon:");
	}
	public void GetAllTradeByAcct(String acct)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(Type.Trade) + $"?acct={acct}")));
		waitResponse(t, "GetAllTradeByAcct:");
	}
	public void GetAllTradeByDate(DateTime timeInUTC)
	{
		var t = Task.Run(() => GetURL(new Uri(baseUri(Type.Trade) + $"?date={timeInUTC:yyyy-MM-dd}")));
		waitResponse(t, "GetAllTradeByDate:");
	}

	public void Get(Type type, String identifier)
	{
		// Identifier for Search --> Acct: acctNum; Strat: id; Trader: name; Trade: id
		var t = Task.Run(() => GetURL(new Uri(baseUri(type) + $"/{identifier}")));
		waitResponse(t, $"Query {type} {identifier}:");
	}

	public void Delete(Type type, String identifier)
	{
		// Identifier for Search --> Acct: acctNum; Strat: id; Trader: name; Trade: id	
		var t = Task.Run(() => DeleteURL(new Uri(baseUri(type) + $"/{identifier}")));
		waitResponse(t, $"Delete {type} {identifier}:");
	}

	public void Exist(Type type, String identifier)
	{
		// Identifier for Search --> Acct: acctNum; Strat: id; Trader: name; Trade: id	
		if (type == Type.Strat){
			Console.WriteLine("Didn't implement this method; thought it's unnecessary");
			return;
		}
		var t = Task.Run(() => OptionsURL(new Uri(baseUri(type) + $"/{identifier}")));
		waitResponse(t, $"Check {type} {identifier} Exist:");
	}

	public void AddAcct(String acctNum, String acctDes, String reportTo)
	{
		add(Type.Acct, JsonConvert.SerializeObject(new { acctDes = acctDes, acctNum = acctNum, reportTo = reportTo }));
	}
	public void EditAcct(String acctNum, String acctDes = null, String reportTo = null)
	{
		edit(Type.Acct, acctNum, JsonConvert.SerializeObject(new { acctDes = acctDes, reportTo = reportTo }));
	}

	public void AddStrat(String id, String stratName, String createBy)
	{
		add(Type.Strat, JsonConvert.SerializeObject(new { id = id, stratName = stratName, createBy = createBy }));
	}
	
	public void AddTrader(String id, String name, String phoneNum, String email){
		add(Type.Trader, JsonConvert.SerializeObject(new { id = id, name = name, phoneNum = phoneNum, email = email }));
	}
	
	public void EditTrader(String name, String id = null, String phoneNum = null, String email = null)
	{
		edit(Type.Trader, name, JsonConvert.SerializeObject(new { id = id, phoneNum = phoneNum, email = email }));
	}
	public void AddTrade(String id, String tradeAcct, String contract, int price, int qty, DateTime timeInUTC)
	{
		add(Type.Trade, JsonConvert.SerializeObject(new { id = id, tradeAcct = tradeAcct, contract = contract, price = price, qty = qty, timeInUTC = $"{timeInUTC:yyyy-MM-dd}" }));
	}

	public void EditTrade(String id, String tradeAcct = null, String contract = null, int? price = null, int? qty = null, DateTime? timeInUTC = null)
	{
		edit(Type.Trade, id, JsonConvert.SerializeObject(new { tradeAcct = tradeAcct, contract = contract, price = price, qty = qty, timeInUTC = timeInUTC.HasValue ? $"{timeInUTC:yyyy-MM-dd}" : null }));
	}

	// helper funcs
	private void startServer()
	{
		var myProcess = new System.Diagnostics.ProcessStartInfo();
		myProcess.UseShellExecute = true;
		myProcess.CreateNoWindow = false;
		myProcess.FileName = "java";
		myProcess.Arguments = "-jar C:\\Users\\yini\\Desktop\\Files\\RestAPI\\db\\target\\dbtest-1.0.1.jar";
		Process.Start(myProcess);
	}

	private Dictionary<String, String> baseURI = new Dictionary<string, string>{
		{"Acct","http://localhost:4567/api/accts"},
		{"Strat","http://localhost:4567/api/strats"},
		{"Trader","http://localhost:4567/api/traders"},
		{"Trade","http://localhost:4567/api/trades"}
	};

	private String baseUri(Type type)
	{
		return baseURI[type.ToString()];
	}
	private void add(Type type, String jsonString)
	{
		HttpContent c = new StringContent(jsonString, Encoding.UTF8, "application/json");
		var t = Task.Run(() => PostURL(new Uri(baseUri(type)), c));
		waitResponse(t, $"Add new {type}:");
	}

	private void edit(Type type, String identifier, String jsonString)
	{
		HttpContent c = new StringContent(jsonString, Encoding.UTF8, "application/json");
		var t = Task.Run(() => PutURL(new Uri(baseUri(type) + $"/{identifier}"), c));
		waitResponse(t, $"Edit {type} {identifier}:");
	}
	
	private void waitResponse(Task<String> t, string reqType)
	{
		t.Wait();
		reqType.Dump();
		JToken jt = JToken.Parse(t.Result);
		string formattedJson = jt.ToString();
		formattedJson.Dump();
		Console.ReadLine();
		Console.WriteLine();
	}
}

public enum Type
{
	Acct = 0,
	Strat = 1,
	Trader = 2,
	Trade = 4
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
