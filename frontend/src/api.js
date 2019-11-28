import axios from 'axios';
import moment from 'moment';
import * as localStorageTokens from './localStorageToken.js';

const api = axios.create({ baseURL: 'https://go-trip.herokuapp.com/', headers: { Accept: 'application/json' } });
const apiGet = axios.create({ baseURL: 'https://go-trip.herokuapp.com/', headers: { 'Content-Type': 'application/x-www-from-urlencoded' } })
const apiPhoto = axios.create({ baseURL: 'http://185.255.96.249:5000/', headers: { 'Content-Type': 'multipart/form-data' } })


export const register = data => api.post('/register', data);
export const login = data => api.post('/authorize', data);
export const getUser = id => apiGet.get(`/user/get?id=${id}`);
export const getCompanyOwner = id => apiGet.get(`/company/get/owner?id=${id}`)
export const updateUser = (data) => api.post('/update/user', data)

export const addUserPhoto = (data) => apiPhoto.post('/fileupload', data)
export const companyPhoto = data => apiPhoto.post(`/company`, data)

export const getCompanyAdmins = id => apiGet.get(`/administrator/get?companyId=${id}`);
export const addAdministrator = (data) => api.post('/administrator/add', data)
export const removeArdimistrator = id => api.post(`/administrator/remove?id=${id}`)

export const becomeGuide = data => api.post(`/guide/add`, data)

export const addNewTour = data => api.post('/tours/add', data)
export const getTours = () => apiGet.get('tours/get')

export const companyRegistration = data => api.post('/company/registration', data)

export const getCompany = id => apiGet.get(`/company/get?id=${id}`)




api.interceptors.request.use(
  config => {
    const token = localStorageTokens.getAccessToken();
    console.log('token', token)
    if (token) {
      //        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  error => {
    Promise.reject(error);
  },
);

let retry = false;

api.interceptors.response.use(
  response => {
    const now = moment(Date.now());
    const expireDate = moment(localStorageTokens.getExpireDate());

    const diff = expireDate.diff(now, 'minutes');
    console.log('diff-', diff, now, expireDate)
    if (diff < 10 && !retry) {
      retry = true;

      const token = localStorageTokens.getRefreshToken();
      /*refreshToken(token).then(res => {
        localStorageTokens.setToken(res.data);
      });*/
    } else {
      retry = false;
    }

    return response;
  },
  error => {
    if (error.response && error.response.status === 401 && localStorageTokens.getAccessToken()) {
      localStorageTokens.clearToken();
      window.location.href = '/';
    }

    return Promise.reject(error);
  },
);
