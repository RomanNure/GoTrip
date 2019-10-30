package org.nure.gotrip.dto;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class AdministratorAddDto {

    private long companyId;
    private String email;
    private String login;

}