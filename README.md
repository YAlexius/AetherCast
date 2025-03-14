# AetherCast - 现代化视频播放器

## 项目简介

AetherCast 是一个基于 Windows App SDK (WinUI 3) 和 C# 开发的现代化视频播放器应用。这是一个个人学习项目，旨在通过实践提升开发技能。

## 预览

![预览 1](./docs/UIpreview1.png)
![预览 2](./docs/UIpreview2.png)

## 系统要求

- 操作系统：Windows 10 版本 1809 (Build 17763.0) 及以上
- 处理器架构：支持 X86-64 或 ARM64

## 安装方式

### 侧载安装

1. 打开系统设置
   - 导航至 `系统` -> `开发者选项`
   - 启用 `开发人员模式`
2. 配置 PowerShell 执行策略
   - 滚动到页面底部
   - 展开 `PowerShell` 区块
   - 开启 `更改执行策略...` 选项
3. 下载安装
   - 从 [Release](https://github.com/YAlexius/AetherCast/releases/) 页面下载最新版本应用包
   - 解压应用包
   - 使用 Windows PowerShell 运行 `Install.ps1`

### Windows 应用商店安装

- **待实现**：上架 Windows 应用商店

## 功能特性

### 已实现功能

#### 视频播放
- 基于 `MediaPlayerElement` 的视频播放核心
- 使用原生 `TransportControls` 媒体传输控件
  - 进度控制
  - 音量调节
  - 快进/快退
  - 投屏支持

#### 用户界面
- 主题适配
  - 深色/浅色模式切换
  - 支持跟随系统主题
- 本地化支持
  - 中文/英文双语界面

### 计划中的功能

#### 播放器增强
- 集成 mpv 播放器内核
- 提供用户可选的播放器内核（原生/mpv）
- 新的自定义媒体传输控件

#### 内容增强
- 弹幕系统
- 播放列表管理

## 许可证

[MIT License](https://github.com/YAlexius/AetherCast/tree/main?tab=MIT-1-ov-file)

## 鸣谢

- [Windows App SDK](https://github.com/microsoft/WindowsAppSDK)

**免责声明**：这是一个学习项目，仍在持续开发中。
