package org.nure.gotrip.controller;

import org.junit.Assert;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mockito;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.RegisteredUserService;
import org.nure.gotrip.util.contstant.UserConstants;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@RunWith(SpringRunner.class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc
public class AuthorizationControllerTest {

	private static final String AUTHORIZE = "/authorize";

	@MockBean
	RegisteredUserService registeredUserService;

	@Autowired
	private MockMvc mvc;

	@Test
	public void shouldReturnUserAfterAuthorization() throws Exception {
		String login = "olololo123";
		String password = "Password123456";
		RegisteredUser registeredUser = new RegisteredUser();
		String expectedJson = "{\"id\":0,\"login\":\"olololo123\",\"email\":null,\"fullName\":null,\"phone\":null,\"registrationDatetime\":null,\"emailConfirmed\":false,\"avatarUrl\":null}";

		registeredUser.setLogin(login);
		registeredUser.setPassword(password);

		Mockito.when(registeredUserService.findByLogin(login)).thenReturn(registeredUser);
		Mockito.when(registeredUserService.checkPassword(registeredUser, password)).thenReturn(true);

		MvcResult mvcResult = mvc.perform(post(AUTHORIZE)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.accept(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk())
				.andReturn();

		String content = mvcResult.getResponse().getContentAsString();
		Assert.assertEquals(expectedJson, content);
	}

	@Test
	public void shouldThrowBadRequestWhenInvalidPassword() throws Exception {
		String login = "olololo123";
		String password = "Password123456";
		RegisteredUser registeredUser = new RegisteredUser();
		registeredUser.setLogin(login);
		registeredUser.setPassword(password);

		Mockito.when(registeredUserService.findByLogin(login)).thenReturn(registeredUser);
		Mockito.when(registeredUserService.checkPassword(registeredUser, password)).thenReturn(false);

		mvc.perform(post(AUTHORIZE)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.accept(MediaType.APPLICATION_JSON))
				.andExpect(status().isBadRequest());
	}

	@Test
	public void shouldThrowNotFoundExceptionWhenLoginIsNotValid() throws Exception {
		String login = "olololo123";
		String password = "Password123456";
		RegisteredUser registeredUser = new RegisteredUser();
		registeredUser.setLogin(login);
		registeredUser.setPassword(password);

		Mockito.when(registeredUserService.findByLogin(login)).thenThrow(new NotFoundUserException("User with such login doesn't exist"));

		mvc.perform(post(AUTHORIZE)
				.param(UserConstants.LOGIN, login)
				.param(UserConstants.PASSWORD, password)
				.accept(MediaType.APPLICATION_JSON))
				.andExpect(status().isNotFound());
	}

}