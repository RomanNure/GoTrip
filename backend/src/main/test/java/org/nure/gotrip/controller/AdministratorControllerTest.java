package org.nure.gotrip.controller;

import com.jayway.jsonpath.spi.json.GsonJsonProvider;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.Mockito;
import org.nure.gotrip.dto.AdministratorAddDto;
import org.nure.gotrip.exception.NotFoundUserException;
import org.nure.gotrip.exception.NotUniqueAdministratorException;
import org.nure.gotrip.model.Administrator;
import org.nure.gotrip.model.Company;
import org.nure.gotrip.model.RegisteredUser;
import org.nure.gotrip.service.AdministratorService;
import org.nure.gotrip.service.CompanyService;
import org.nure.gotrip.service.RegisteredUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@RunWith(SpringRunner.class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc
public class AdministratorControllerTest {

	@Autowired
	private MockMvc mvc;

	@MockBean
	private AdministratorService administratorService;

	@MockBean
	private RegisteredUserService registeredUserService;

	@MockBean
	private CompanyService companyService;

	@Test
	public void shouldReturnNotFoundResult() throws Exception {
		AdministratorAddDto administratorDto = new AdministratorAddDto();
		RegisteredUser registeredUser = Mockito.mock(RegisteredUser.class);
		Company company = Mockito.mock(Company.class);
		Administrator administrator = Mockito.mock(Administrator.class);
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		//administratorDto.setUserId(1L);
		administratorDto.setCompanyId(1L);

		Mockito.when(registeredUserService.findById(1L)).thenReturn(registeredUser);
		Mockito.when(companyService.findById(1L)).thenReturn(company);
		Mockito.when(administratorService.addAdministrator(administrator)).thenReturn(administrator);

		mvc.perform(post("/administrator/add")
				.content(gsonJsonProvider.toJson(administratorDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk());
	}

	@Test
	public void shouldReturnNotFoundWhenTryToAddAdministrator() throws Exception {
		AdministratorAddDto administratorDto = new AdministratorAddDto();
		RegisteredUser registeredUser = Mockito.mock(RegisteredUser.class);
		Company company = Mockito.mock(Company.class);
		Administrator administrator = Mockito.mock(Administrator.class);
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		//administratorDto.setUserId(1L);
		administratorDto.setCompanyId(1L);

		Mockito.when(registeredUserService.findById(1L)).thenThrow(NotFoundUserException.class);
		Mockito.when(companyService.findById(1L)).thenReturn(company);
		Mockito.when(administratorService.addAdministrator(administrator)).thenReturn(administrator);

		mvc.perform(post("/administrator/add")
				.content(gsonJsonProvider.toJson(administratorDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isNotFound());
	}

	@Test
	public void shouldReturnConflictWhenTryToAddAdministrator() throws Exception {
		AdministratorAddDto administratorDto = new AdministratorAddDto();
		RegisteredUser registeredUser = Mockito.mock(RegisteredUser.class);
		Company company = Mockito.mock(Company.class);
		Administrator administrator = Mockito.mock(Administrator.class);
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		//administratorDto.setUserId(1L);
		administratorDto.setCompanyId(1L);

		Mockito.when(registeredUserService.findById(1L)).thenReturn(registeredUser);
		Mockito.when(companyService.findById(1L)).thenReturn(company);
		Mockito.when(administratorService.addAdministrator(administrator)).thenThrow(NotUniqueAdministratorException.class);

		mvc.perform(post("/administrator/add")
				.content(gsonJsonProvider.toJson(administratorDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isConflict());
	}
}