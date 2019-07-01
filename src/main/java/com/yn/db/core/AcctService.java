package com.yn.db.core;

import java.util.Collection;

public interface AcctService {
    public void addAcct(Acct acct);

    public Collection<Acct> getAccts();

    public Collection<Acct> getAccts(String trader);

    public Acct getAcct(String acctNum);

    public Acct editAcct(Acct acct) throws AcctException;

    public void deleteAcct(String acctNum);

    public boolean acctExist(String acctNum);
}
