package com.yn.db.core;

import java.util.Collection;
import java.util.HashMap;
import java.util.ArrayList;

public class AcctServiceMapImpl implements AcctService {
    private HashMap<String, Acct> acctMap;

    public AcctServiceMapImpl() {
        acctMap = new HashMap<>();
    }

    @Override
    public void addAcct(Acct acct) {
        acctMap.put(acct.getAcctNum(), acct);
    }

    @Override
    public Collection<Acct> getAccts() {
        return acctMap.values();
    }

    @Override
    public Collection<Acct> getAccts(String trader) {
        Collection<Acct> res = new ArrayList<Acct>();
        for (Acct a : acctMap.values()){
            if (a.getReportTo().equals(trader)){
                res.add(a);
            }
        }      
        return res;
    }

    @Override
    public Acct getAcct(String acctNum) {
        return acctMap.get(acctNum);
    }

    @Override
    public Acct editAcct(Acct forEdit) throws AcctException {
        try {
            if (forEdit.getAcctNum() == null)
                throw new AcctException("acct num cannot be blank");

            Acct toEdit = acctMap.get(forEdit.getAcctNum());

            if (toEdit == null)
                throw new AcctException("Acct not found");

            if (forEdit.getAcctDes() != null) {
                toEdit.setAcctDes(forEdit.getAcctDes());
            }
            if (forEdit.getReportTo() != null) {
                toEdit.setReportTo(forEdit.getReportTo());
            }
            /*if (forEdit.getId() != null) {
                toEdit.setId(forEdit.getId());
            }*/

            return toEdit;
        } catch (Exception ex) {
            throw new AcctException(ex.getMessage());
        }
    }

    @Override
    public void deleteAcct(String acctNum) {
        acctMap.remove(acctNum);
    }

    @Override
    public boolean acctExist(String acctNum) {
        return acctMap.containsKey(acctNum);
    }

}
