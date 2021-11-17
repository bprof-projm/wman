import React from 'react';
import {useHistory} from "react-router-dom";
import { Button } from 'antd';
import { PrinterOutlined } from '@ant-design/icons';
const PrintButton = () =>
{
    const history = useHistory();

    function handleClick() {
        history.push('/print');
    }
    return(
        <div>
        <Button shape='circle' icon={<PrinterOutlined />} text="Print" onClick ={handleClick}/>
        </div>
    )
}
export default PrintButton;