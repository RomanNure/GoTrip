package org.nure.gotrip.model.notification;

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
    private String id;

    @Column(name = "type")
    private String type;

    @Column(name="topic")
    private String topic;

    @Column(name="checked")
    private boolean checked;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "registered_user_id")
    private RegisteredUser user;

    @Column(name="data")
    private String data;

    public Notification(NotificationData notificationData){
        try {
            data = notificationData.data();
        } catch (JsonProcessingException e) {
            throw new SerializationException(e.getMessage());
        }
        type = notificationData.getType();
    }

    public String getId() {
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
