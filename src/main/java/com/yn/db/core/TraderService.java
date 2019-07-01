package com.yn.db.core;

import java.util.Collection;

public interface TraderService {
    public void addTrader(Trader trader);

    public Collection<Trader> getTraders();

    public Trader getTrader(String name);

    public Trader editTrader(Trader trader) throws TraderException;

    public void deleteTrader(String name);

    public boolean traderExist(String name);
}
