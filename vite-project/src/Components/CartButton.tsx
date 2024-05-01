import 'bootstrap/dist/css/bootstrap.min.css';
import { Button } from "react-bootstrap";
import { useAppDispatch, useAppSelector } from '../Redux/Hooks';
import { increment, decrement } from '../Redux/CounterSlice';
import React, { useState } from 'react';

interface Button1Props {
    state: boolean;
    textBuy: string;
    textBought: string;
    fun: React.Dispatch<React.SetStateAction<boolean>>;
    inCartNum: number;
    isFavourite: boolean;
}



export function CartButton({ state, fun, textBuy, textBought, inCartNum, isFavourite }: Button1Props) {
    const variant = state ? 'success' : 'primary';
    const text = state ? textBought : textBuy;
    const num = inCartNum;
    const value = useAppSelector((state) => state.counter.value);
    const dispatch = useAppDispatch();
    console.log(inCartNum);
    if (state) {
        return (
            <>
                <Button variant={variant} onClick={() => { }}>{textBought}</Button>
                <Button variant="primary" onClick={() => {inCartNum += 1; textBought = "В корзине" + {inCartNum}; fun(state);}}>
                    +
                </Button>
                <Button variant="danger" onClick={() => {inCartNum -= 1; textBought = "В корзине" + {inCartNum} }}>
                    -
                </Button>
            </>
        );
    }
    if (!state) {
        return (
            <Button variant={variant} onClick={() => { fun(!state); inCartNum += 1; console.log(num)}}>{text}</Button>
        );
    }
}
