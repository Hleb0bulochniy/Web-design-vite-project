import { PayloadAction, createSlice } from "@reduxjs/toolkit"

export interface SumState {
    value: number;
  }
  
  const initialState: SumState = {
    value: 0,
  };

export const SumSlice = createSlice({
    name: 'sum',
    initialState,
    reducers: {
      ChangeSum: (state, num: PayloadAction<number>) => {
        state.value = num.payload;
      },
    }
  })

  export const { ChangeSum } = SumSlice.actions