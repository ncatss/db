package com.yn.db.core;

import java.util.Collection;
import java.util.HashMap;
import java.sql.Date;
import java.util.ArrayList;

public class TradeServiceMapImpl implements TradeService {
    private HashMap<String, Trade> tradeMap;

    public TradeServiceMapImpl() {
        tradeMap = new HashMap<>();
    }

    @Override
    public void addTrade(Trade trade) {
        tradeMap.put(trade.getId(), trade);
    }

    @Override
    public Collection<Trade> getTrades() {
        return tradeMap.values();
    }

    @Override
    public Collection<Trade> getTradesByContract(String con) {
        Collection<Trade> res = new ArrayList<Trade>();
        for (Trade t : tradeMap.values()){
            if (t.getContract().equals(con)){
                res.add(t);
            }
        }      
        return res;
    }

    @Override
    public Collection<Trade> getTradesByAcct(String acct) {
        Collection<Trade> res = new ArrayList<Trade>();
        for (Trade t : tradeMap.values()){
            if (t.getTradeAcct().equals(acct)){
                res.add(t);
            }
        }      
        return res;
    }

    @Override
    public Collection<Trade> getTradesByDate(Date date) {
        Collection<Trade> res = new ArrayList<Trade>();
        for (Trade t : tradeMap.values()){
            if (t.getTimeInUTC().compareTo(date) == 0){
                res.add(t);
            }
        }      
        return res;
    }

    @Override
    public Trade getTrade(String id) {
        return tradeMap.get(id);
    }

    @Override
    public Trade editTrade(Trade forEdit) throws TradeException {
        try {
            if (forEdit.getId() == null)
                throw new TradeException("ID cannot be blank");

            Trade toEdit = tradeMap.get(forEdit.getId());

            if (toEdit == null)
                throw new TradeException("Trade not found");

            if (forEdit.getTradeAcct() != null) {
                toEdit.setTradeAcct(forEdit.getTradeAcct());
            }
            if (forEdit.getContract() != null) {
                toEdit.setContract(forEdit.getContract());
            }
            if (forEdit.getPrice() != null) {
                toEdit.setPrice(forEdit.getPrice());
            }
            if (forEdit.getQty() != null) {
                toEdit.setQty(forEdit.getQty());
            }
            if (forEdit.getTimeInUTC() != null) {
                toEdit.setTimeInUTC(forEdit.getTimeInUTC());
            }

            return toEdit;
        } catch (Exception ex) {
            throw new TradeException(ex.getMessage());
        }
    }

    @Override
    public void deleteTrade(String id) {
        tradeMap.remove(id);
    }

    @Override
    public boolean tradeExist(String id) {
        return tradeMap.containsKey(id);
    }

}
