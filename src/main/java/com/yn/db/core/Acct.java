/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.yn.db.core;

public class Acct {

    //private String id;
    private String acctNum;
    private String acctDes;
    private String reportTo; // to trader

    public Acct(/*String id, */String acctNum, String acctDes, String reportTo) {
        super();
        //this.id = id;
        this.acctNum = acctNum;
        this.acctDes = acctDes;
        this.reportTo = reportTo;
    }

    /*public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }*/

    public String getAcctNum() {
        return acctNum;
    }

    public void setAcctNum(String acctNum) {
        this.acctNum = acctNum;
    }

    public String getAcctDes() {
        return acctDes;
    }

    public void setAcctDes(String acctDes) {
        this.acctDes = acctDes;
    }

    public String getReportTo() {
        return reportTo;
    }

    public void setReportTo(String reportTo) {
        this.reportTo = reportTo;
    }

    @Override
    public String toString() {
        return new StringBuffer().append(getReportTo()).toString();
    }
}
