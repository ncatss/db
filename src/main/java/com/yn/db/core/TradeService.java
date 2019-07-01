package com.yn.db.core;

import java.sql.Date;
import java.util.Collection;

public interface TradeService {
    public void addTrade(Trade trade);

    public Collection<Trade> getTrades();
    public Collection<Trade> getTradesByContract(String con);
    public Collection<Trade> getTradesByAcct(String acct);
    public Collection<Trade> getTradesByDate(Date date);

    public Trade getTrade(String id);

    public Trade editTrade(Trade trade) throws TradeException;

    public void deleteTrade(String id);

    public boolean tradeExist(String id);
}
