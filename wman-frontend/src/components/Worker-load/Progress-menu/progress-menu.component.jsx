import { Button, Drawer } from "antd";
import React, { useState } from "react";
import { PieChartOutlined } from '@ant-design/icons';

const ProgressMenu = () =>{
    const [visible, setVisible] = useState(false);

    const showMenu = () =>{
        setVisible(true);
    }

    const closeMenu = () =>{
        setVisible(false);
    }
    
    return(
        <div>
        <Button shape="circle" icon={<PieChartOutlined />} onClick={showMenu}></Button>
        <Drawer title="Progress Menu" placement="right" onClose={closeMenu} visible={visible}>
        <p>szöveeeeeeeeeg</p>
        </Drawer>
        </div>
    )
}

export default ProgressMenu;