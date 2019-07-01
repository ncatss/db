package com.yn.db.core;

import java.util.Collection;
import java.util.HashMap;
import java.util.ArrayList;

public class StratServiceMapImpl implements StratService {
    private HashMap<String, Strat> stratMap;

    public StratServiceMapImpl() {
        stratMap = new HashMap<>();
    }

    @Override
    public void addStrat(Strat strat) {
        stratMap.put(strat.getId(), strat);
    }

    @Override
    public Collection<Strat> getStrats() {
        return stratMap.values();
    }

    @Override
    public Collection<Strat> getStrats(String trader) {
        Collection<Strat> res = new ArrayList<Strat>();
        for (Strat s : stratMap.values()){
            if (s.getCreateBy().equals(trader)){
                res.add(s);
            }
        }      
        return res;
    }

    @Override
    public Strat getStrat(String id) {
        return stratMap.get(id);
    }

    @Override
    public void deleteStrat(String id) {
        stratMap.remove(id);
    }
}
