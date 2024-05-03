import { login, loginRequest, loginFailure } from "./AuthSlice";

interface AuthPayload {
  email: string;
  password: string;
}

export const auth = (payload: AuthPayload) => async (dispatch: any) => {
  dispatch(loginRequest());
  try {
    const response = await fetch('https://localhost:7287/UserLoginWeb', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });
    const result = await response.json();
    if (response.ok) {
      dispatch(login({ accessToken: result.access_token, refreshToken: result.refresh_token, username: result.username }));
      localStorage.setItem('accessToken', result.access_token);
      window.location.href = "/home"
    } else {
      dispatch(loginFailure(result.error || 'Registration failed'));
    }
  } catch (error) {
    dispatch(loginFailure('Registration failed'));
  }
};