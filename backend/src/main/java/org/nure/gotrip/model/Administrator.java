package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToMany;
import javax.persistence.Table;
import java.util.List;

@Getter
@Setter
@Entity
@Table(name = "administrators")
public class Administrator {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "administrator_id")
    private long id;

    @JsonIgnore
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "registered_user_id")
    private RegisteredUser registeredUser;

    @JsonIgnore
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "company_id")
    private Company company;

    @JsonIgnore
    @OneToMany(fetch = FetchType.LAZY, mappedBy = "administrator")
    private List<Tour> tours;

    public Administrator(){
        //Constructor for JPA
    }

    public Administrator(long id){
        this.id = id;
    }

    public Administrator(RegisteredUser user, Company company){
        this.registeredUser = user;
        this.company = company;
    }
}
