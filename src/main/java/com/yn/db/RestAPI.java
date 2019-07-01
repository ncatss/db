package com.yn.db;

import com.yn.db.core.*;
import static spark.Spark.*;
import com.google.gson.Gson;
import java.sql.Date;

public class RestAPI {

    public static void main(String[] args) {
        final AcctService acctService = new AcctServiceMapImpl();
        final StratService stratService = new StratServiceMapImpl();
        final TraderService traderService = new TraderServiceMapImpl();
        final TradeService tradeService = new TradeServiceMapImpl();

        path("/api", () -> {
            //before("/*",(q,a) -> log.info("Received api call"));
            path("/accts", () -> {
                get("", (request, response) -> {
                    response.type("application/json");
                    if (request.queryParams("traderName") != null)
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(acctService.getAccts(request.queryParams("traderName")))));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(acctService.getAccts())));
                });
                get("/:acctNum", (request, response) -> {
                    response.type("application/json");      
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(acctService.getAcct(request.params(":acctNum")))));
                });
                post("", (request, response) -> {
                    response.type("application/json");
        
                    Acct acct = new Gson().fromJson(request.body(), Acct.class);
                    acctService.addAcct(acct);
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(acctService.getAccts())));
                });
                put("/:acctNum", (request, response) -> {
                    response.type("application/json");
        
                    Acct toEdit = new Gson().fromJson(request.body(), Acct.class);
                    if (toEdit.getAcctNum() == null)
                        toEdit.setAcctNum(request.params(":acctNum"));
                    Acct editedAcct = acctService.editAcct(toEdit);
        
                    if (editedAcct != null) {
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(editedAcct)));
                    } else {
                        return new Gson().toJson(new StandardResponse(StatusResponse.ERROR, new Gson().toJson("Acct "+ request.params(":acctNum") + " not found or error in edit")));
                    }
                });
                delete("/:acctNum", (request, response) -> {
                    response.type("application/json");
        
                    acctService.deleteAcct(request.params(":acctNum"));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Acct " + request.params(":acctNum") +" deleted"));
                });
                options("/:acctNum", (request, response) -> {
                    response.type("application/json");
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Acct "+ request.params(":acctNum") + ((acctService.acctExist(request.params(":acctNum"))) ? " exists" : " does not exists")));
                });
            });
        
            path("/strats", () -> {
                post("", (request, response) -> {
                    response.type("application/json");
        
                    Strat strat = new Gson().fromJson(request.body(), Strat.class);
                    stratService.addStrat(strat);
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(stratService.getStrats())));
                });
                get("", (request, response) -> {
                    response.type("application/json");
                    if (request.queryParams("traderName") != null)
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(stratService.getStrats(request.queryParams("traderName")))));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(stratService.getStrats())));
                });
                get("/:id", (request, response) -> {
                    response.type("application/json");      
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(stratService.getStrat(request.params(":id")))));
                });
                delete("/:id", (request, response) -> {
                    response.type("application/json");       
                    stratService.deleteStrat(request.params(":id"));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Strat "+ request.params(":id") + " deleted"));
                });
            });
            
            path("/traders", () -> {
                post("", (request, response) -> {
                    response.type("application/json");
        
                    Trader trader = new Gson().fromJson(request.body(), Trader.class);
                    traderService.addTrader(trader);
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(traderService.getTraders())));
                });
                get("", (request, response) -> {
                    response.type("application/json");
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(traderService.getTraders())));
                });
                get("/:name", (request, response) -> {
                    response.type("application/json");      
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(traderService.getTrader(request.params(":name")))));
                });
                put("/:name", (request, response) -> {
                    response.type("application/json");
        
                    Trader toEdit = new Gson().fromJson(request.body(), Trader.class);
                    if (toEdit.getName() == null )
                        toEdit.setName(request.params(":name"));
                    Trader editedTrader = traderService.editTrader(toEdit);
        
                    if (editedTrader != null) {
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(editedTrader)));
                    } else {
                        return new Gson().toJson(new StandardResponse(StatusResponse.ERROR, new Gson().toJson("Trader " + request.params(":name") + " not found or error in edit")));
                    }
                });
                delete("/:name", (request, response) -> {
                    response.type("application/json");
        
                    traderService.deleteTrader(request.params(":name"));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Trader "+ request.params(":name")+ " deleted"));
                });
                options("/:name", (request, response) -> {
                    response.type("application/json");
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Trader "+ request.params(":name") + ((traderService.traderExist(request.params(":name"))) ? " exists" : " does not exists")));
                });
            });

            path("/trades", () -> {
                post("", (request, response) -> {
                    response.type("application/json");
        
                    Trade trade = new Gson().fromJson(request.body(), Trade.class);
                    tradeService.addTrade(trade);
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTrades())));
                });
                get("", (request, response) -> {
                    response.type("application/json");
                    if (request.queryParams("con") != null)
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTradesByContract(request.queryParams("con")))));
                    if (request.queryParams("acct") != null)
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTradesByAcct(request.queryParams("acct")))));
                    if (request.queryParams("date") != null)
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTradesByDate(Date.valueOf(request.queryParams("date"))))));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTrades())));
                });
                get("/:id", (request, response) -> {
                    response.type("application/json");      
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(tradeService.getTrade(request.params(":id")))));
                });
                put("/:id", (request, response) -> {
                    response.type("application/json");
        
                    Trade toEdit = new Gson().fromJson(request.body(), Trade.class);
                    if (toEdit.getId() == null )
                        toEdit.setId(request.params(":id"));
                    Trade editedTrade = tradeService.editTrade(toEdit);
        
                    if (editedTrade != null) {
                        return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, new Gson().toJsonTree(editedTrade)));
                    } else {
                        return new Gson().toJson(new StandardResponse(StatusResponse.ERROR, new Gson().toJson("Trade " + request.params(":id") + " not found or error in edit")));
                    }
                });
                delete("/:id", (request, response) -> {
                    response.type("application/json");       
                    tradeService.deleteTrade(request.params(":id"));
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Trade "+ request.params(":id") + " deleted"));
                });
                options("/:id", (request, response) -> {
                    response.type("application/json");
        
                    return new Gson().toJson(new StandardResponse(StatusResponse.SUCCESS, "Trade "+ request.params(":id") + ((tradeService.tradeExist(request.params(":id"))) ? " exists" : " does not exists")));
                });
            });
        });
    }

}
