package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.nure.gotrip.model.Guide;
import org.nure.gotrip.model.RegisteredUser;

import java.math.BigInteger;
import java.util.Date;

@Getter
@Setter
@NoArgsConstructor
public class RegisteredUserDto {

    private long id;
    private String login;
    private String email;
    private String fullName;
    private String phone;
    private Date registrationDatetime;
    private boolean emailConfirmed;
    private String avatarUrl;
    private String description;

    private Iterable<BigInteger> administrators;
	private Iterable<BigInteger> companies;
	private Guide guide;

	public void setRegisteredUser(RegisteredUser user){
        this.id = user.getId();
        this.login = user.getLogin();
        this.email=user.getEmail();
        this.fullName=user.getFullName();
        this.phone=user.getPhone();
        this.registrationDatetime=user.getRegistrationDatetime();
        this.emailConfirmed=user.isEmailConfirmed();
        this.avatarUrl=user.getAvatarUrl();
        this.description=user.getDescription();
    }
}