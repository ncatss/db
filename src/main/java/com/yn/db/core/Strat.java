/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.yn.db.core;

public class Strat {

    private String id;
    private String stratName;
    private String createBy;

    public Strat(String id, String stratName, String createBy) {
        super();
        this.id = id;
        this.stratName = stratName;
        this.createBy = createBy;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getStratName() {
        return stratName;
    }

    public void setStratName(String stratName) {
        this.stratName = stratName;
    }

    public String getCreateBy() {
        return createBy;
    }

    public void setCreateBy(String createBy) {
        this.createBy = createBy;
    }

    @Override
    public String toString() {
        return new StringBuffer().append(getStratName()).toString();
    }
}
