package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;
import org.nure.gotrip.model.RegisteredUser;

import java.util.Date;

@Getter
@Setter
public class AdministratorDto {

    private long administratorId;
    private long id;
    private String login;
    private String email;
    private String fullName;
    private String phone;
    private Date registrationDatetime;
    private boolean emailConfirmed;
    private String avatarUrl;
    private String description;

    public AdministratorDto(long administratorId, RegisteredUser user){
        this.administratorId = administratorId;
        this.id = user.getId();
        this.login = user.getLogin();
        this.email = user.getEmail();
        this.fullName = user.getFullName();
        this.phone = user.getPhone();
        this.registrationDatetime = user.getRegistrationDatetime();
        this.emailConfirmed = user.isEmailConfirmed();
        this.avatarUrl = user.getAvatarUrl();
        this.description = user.getDescription();
    }
}
