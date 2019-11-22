package org.nure.gotrip.model.notification;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.core.JsonProcessingException;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.nure.gotrip.exception.SerializationException;
import org.nure.gotrip.model.RegisteredUser;

import javax.persistence.*;

@Setter
@NoArgsConstructor
@Entity
@Table(name = "notifications")
public class Notification {

    @Id
    @Column(name = "notification_id")
    private long id;

    @Column(name = "type")
    private String type;

    @Column(name="topic")
    private String topic;

    @Column(name="checked")
    private boolean checked;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "registered_user_id")
    private RegisteredUser user;

    @Column(name="data")
    private String data;

    private Notification(NotificationData notificationData){
        try {
            data = notificationData.getData();
        } catch (JsonProcessingException e) {
            throw new SerializationException(e.getMessage());
        }
        type = notificationData.getType();
    }

    public long getId() {
        return id;
    }

    public String getType() {
        return type;
    }

    public String getTopic() {
        return topic;
    }

    public boolean isChecked() {
        return checked;
    }

    public RegisteredUser getUser() {
        return user;
    }

    public String getData() {
        return data;
    }
}
