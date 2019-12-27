package org.nure.gotrip.model;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.*;
import java.util.Date;

@Getter
@Setter
@Entity
@Table(name = "messages")
public class Message {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "message_id")
    private long id;

    @Column(name="text")
    private String text;

    @Temporal(TemporalType.TIMESTAMP)
    @Column(name = "datetime")
    private Date dateTime;

    @Column(name = "read")
    private boolean read;

    @JsonIgnoreProperties("messages")
    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "chat_id")
    private Chat chat;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name="author_id")
    private RegisteredUser author;
}
