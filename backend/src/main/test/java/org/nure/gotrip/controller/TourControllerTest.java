package org.nure.gotrip.controller;

import com.jayway.jsonpath.spi.json.GsonJsonProvider;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.nure.gotrip.dto.TourDto;
import org.nure.gotrip.repository.AdministratorRepository;
import org.nure.gotrip.repository.TourRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.http.MediaType;
import org.springframework.test.context.junit4.SpringRunner;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders;
import org.springframework.test.web.servlet.result.MockMvcResultMatchers;

import java.util.Date;

import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@RunWith(SpringRunner.class)
@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@AutoConfigureMockMvc
public class TourControllerTest {

	@MockBean
	private TourRepository tourRepository;

	@MockBean
	private AdministratorRepository administratorRepository;

	@Autowired
	private MockMvc mvc;

	@Test
	public void shouldReturnPositiveResponseWithJsonListWithTours() throws Exception {
		mvc.perform(MockMvcRequestBuilders.get("/tours/get")
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(MockMvcResultMatchers.status().isOk())
				.andReturn();
	}

	@Test
	public void shouldReturnPositiveResponseWhenAddTour() throws Exception {
		TourDto tourDto = new TourDto();
		Date date = new Date();
		GsonJsonProvider gsonJsonProvider = new GsonJsonProvider();

		tourDto.setDescription("one");
		tourDto.setIdAdministrator(1);
		tourDto.setName("name");
		tourDto.setMainPictureUrl("");
		tourDto.setMaxParticipants(20);
		tourDto.setStartDateTime(date);
		tourDto.setFinishDateTime(date);

		mvc.perform(MockMvcRequestBuilders.post("/tours/add")
				.content(gsonJsonProvider.toJson(tourDto))
				.contentType(MediaType.APPLICATION_JSON))
				.andExpect(status().isOk());
	}

}