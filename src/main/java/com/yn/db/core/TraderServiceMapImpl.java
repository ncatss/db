package com.yn.db.core;

import java.util.Collection;
import java.util.HashMap;
import java.util.ArrayList;

public class TraderServiceMapImpl implements TraderService {
    private HashMap<String, Trader> traderMap;

    public TraderServiceMapImpl() {
        traderMap = new HashMap<>();
    }

    @Override
    public void addTrader(Trader trader) {
        traderMap.put(trader.getName(), trader);
    }

    @Override
    public Collection<Trader> getTraders() {
        return traderMap.values();
    }

    @Override
    public Trader getTrader(String name) {
        return traderMap.get(name);
    }

    @Override
    public Trader editTrader(Trader forEdit) throws TraderException {
        try {
            if (forEdit.getName() == null)
                throw new TraderException("Name cannot be blank");

            Trader toEdit = traderMap.get(forEdit.getName());

            if (toEdit == null)
                throw new TraderException("Trader not found");

            if (forEdit.getEmail() != null) {
                toEdit.setEmail(forEdit.getEmail());
            }
            if (forEdit.getPhoneNum() != null) {
                toEdit.setPhoneNum(forEdit.getPhoneNum());
            }
            if (forEdit.getId() != null) {
                toEdit.setId(forEdit.getId());
            }

            return toEdit;
        } catch (Exception ex) {
            throw new TraderException(ex.getMessage());
        }
    }

    @Override
    public void deleteTrader(String name) {
        traderMap.remove(name);
    }

    @Override
    public boolean traderExist(String name) { 
        return traderMap.containsKey(name);
    }

}
