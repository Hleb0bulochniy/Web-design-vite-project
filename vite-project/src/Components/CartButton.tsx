import 'bootstrap/dist/css/bootstrap.min.css';
import { Button } from "react-bootstrap";
import { useAppDispatch, useAppSelector } from '../Redux/Hooks';
import { increment, decrement } from '../Redux/CounterSlice';
import React, { useState } from 'react';

interface Button1Props {
    state: boolean;
    rand: boolean;
    textBuy: string;
    textBought: string;
    fun: React.Dispatch<React.SetStateAction<boolean>>;
    funAdd: React.Dispatch<React.SetStateAction<boolean>>;
    funRem: React.Dispatch<React.SetStateAction<boolean>>;
    inCartNum: number;
    isFavourite: boolean;
    id: number;
}



export function CartButton({ state, fun, funAdd, funRem, textBuy, textBought, inCartNum, isFavourite, id , rand}: Button1Props) {
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
                <Button variant="primary" onClick={() => {inCartNum += 1; textBought = "В корзине" + {inCartNum}; funAdd(!rand);}}>
                    +
                </Button>
                <Button variant="danger" onClick={() => {inCartNum -= 1; textBought = "В корзине" + {inCartNum}; funRem(!rand); }}>
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
