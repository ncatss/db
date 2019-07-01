/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.yn.db.core;

import java.sql.Date;

public class Trade {

    private String id;
    private String tradeAcct;
    private String contract;
    private Integer price;
    private Integer qty;
    private Date timeInUTC;

    public Trade(String id, String tradeAcct, String contract, Integer price, Integer qty, Date timeInUTC) {
        super();
        this.id = id;
        this.tradeAcct = tradeAcct;
        this.contract = contract;
        this.price = price;
        this.qty = qty;
        this.timeInUTC = timeInUTC;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getTradeAcct() {
        return tradeAcct;
    }

    public void setTradeAcct(String tradeAcct) {
        this.tradeAcct = tradeAcct;
    }

    public String getContract() {
        return contract;
    }

    public void setContract(String contract) {
        this.contract = contract;
    }

    public Integer getPrice() {
        return price;
    }

    public void setPrice(Integer price) {
        this.price = price;
    }

    public Integer getQty(){
        return qty;
    }

    public void setQty(Integer qty){
        this.qty = qty;
    }

    public Date getTimeInUTC(){
        return timeInUTC;
    }

    public void setTimeInUTC(Date timeInUTC){
        this.timeInUTC = timeInUTC;
    }

    @Override
    public String toString() {
        return new StringBuffer().append(getContract()).toString();
    }
}
