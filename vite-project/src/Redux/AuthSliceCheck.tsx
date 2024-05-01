import { createSlice } from '@reduxjs/toolkit';

interface Check {
  isLoading: boolean;
}

const initialState: Check = {
  isLoading: false,
};



const Check = createSlice({
  name: 'Check',
  initialState,
  reducers: {
    CheckRequest(state) {
      state.isLoading = true;
    },
    CheckSuccess(state) {
      state.isLoading = false;
    },
    CheckFailure(state) {
      state.isLoading = false;
    },
  },
});

export const { CheckRequest, CheckSuccess, CheckFailure } = Check.actions;

export default Check.reducer;