package com.yn.db.core;

import java.util.Collection;

public interface StratService {
    public void addStrat(Strat strat);

    public Collection<Strat> getStrats();
    public Collection<Strat> getStrats(String trader);

    public Strat getStrat(String id);
    public void deleteStrat(String id);
}
