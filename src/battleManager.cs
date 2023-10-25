using Godot;
using System;

public partial class battleManager : Node
{
    int maxPlayerHP = 50, PlayerHP;
    int maxEnemyHP = 30, EnemyHP;

    int playerDMG = 20;
    int enemyDMG = 1000;

    bool playerTurn = true;
    bool inBattle = true;
    bool playerIsAlive = true;

    Panel textBox;
    Panel actionMenu;
    Panel gameOverScreen;

    TextureProgressBar playerHealthBar;
    TextureProgressBar enemyHealthBar;

    Label playerHealthText;
    Label enemyHealthText;
    public override void _Ready()
    {
        base._Ready();

        InitializeBattle();
    }

    public void InitializeBattle()
    {
        PlayerHP = maxPlayerHP;
        EnemyHP = maxEnemyHP;

        textBox = GetNode<Panel>("./HUD/TextBox");
        actionMenu = GetNode<Panel>("./HUD/Actions");
        gameOverScreen = GetNode<Panel>("./HUD/GameOver");

        UpdateTextBoxLabel(">A wild cube appears");

        playerHealthBar = GetNode<TextureProgressBar>("./Player/HealthBar/Bar");
        enemyHealthBar = GetNode<TextureProgressBar>("./Fighter/HealthBar/Bar");

        playerHealthBar.MaxValue = maxPlayerHP;
        playerHealthBar.Value = maxPlayerHP;

        enemyHealthBar.MaxValue = maxEnemyHP;
        enemyHealthBar.Value = maxEnemyHP;

        playerHealthText = GetNode<Label>("./Player/HealthBar/Bar/Label");
        enemyHealthText = GetNode<Label>("./Fighter/HealthBar/Bar/Label");

        playerHealthText.Text = PlayerHP + "/" + maxPlayerHP;
        enemyHealthText.Text = EnemyHP + "/" + maxEnemyHP;

        HideGameOver();
    }

    public void ResetBattle()
    {
        PlayerHP = maxPlayerHP;
        EnemyHP = maxEnemyHP;

        playerHealthBar.MaxValue = maxPlayerHP;
        playerHealthBar.Value = maxPlayerHP;

        enemyHealthBar.MaxValue = maxEnemyHP;
        enemyHealthBar.Value = maxEnemyHP;

        playerHealthText.Text = PlayerHP + "/" + maxPlayerHP;
        enemyHealthText.Text = EnemyHP + "/" + maxEnemyHP;

        inBattle = true;

        UpdateTextBoxLabel(">A wild cube appears");
        ShowMessage();

        HideGameOver();
    }

    public void CombatCheck()
    {
        if (inBattle)
        {
            if (!playerTurn && actionMenu.Visible)
            {
                EnemyAttack();
            }
        }
    }

    public void PlayerAttack()
    {
        UpdateTextBoxLabel(">The player deals " + playerDMG + " damage to the red cube.");
        ShowMessage();
        EnemyHP = DealDamage(EnemyHP, playerDMG);
        enemyHealthText = UpdateHealthDisplayText(enemyHealthText, EnemyHP, maxEnemyHP);
        enemyHealthBar = UpdateHealthDisplay(enemyHealthBar, EnemyHP);
        playerTurn = false;
    }

    public void EnemyAttack()
    {
        UpdateTextBoxLabel(">The enemy deals " + enemyDMG + " damage to the player.");
        ShowMessage();
        PlayerHP = DealDamage(PlayerHP, enemyDMG);
        playerHealthText = UpdateHealthDisplayText(playerHealthText, PlayerHP, maxPlayerHP);
        playerHealthBar = UpdateHealthDisplay(playerHealthBar, PlayerHP);
        playerTurn = true;
    }

    public void UpdateTextBoxLabel(string text)
    {
        var textBoxLabel = textBox.GetChild<Label>(0);
        textBoxLabel.Text = text;
    }

    public Label UpdateHealthDisplayText(Label display, int currentHP, int maxHP)
    {
        display.Text = currentHP + "/" + maxHP;
        return display;
    }

    public TextureProgressBar UpdateHealthDisplay(TextureProgressBar display, int currentHP)
    {
        display.Value = currentHP;
        return display;
    }

    public int DealDamage(int targetHP, int damage)
    {
        targetHP -= damage;
        if(targetHP < 0)
        {
            targetHP = 0;
        }
        return targetHP;
    }

    public void ShowMessage()
    {
        actionMenu.Visible = false;
        textBox.Visible = true;
    }

    public void HideMessage()
    {
        if (inBattle)
        {
            textBox.Visible = false;
            actionMenu.Visible = true;

            if (PlayerHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The player's vision starts to blur. He loses counciousness as he falls into the ground.");
                inBattle = false;
                playerIsAlive = false;
            }
            else if (EnemyHP <= 0)
            {
                ShowMessage();
                UpdateTextBoxLabel(">The cube breaks down like glass, disapearing after a few moments.");
                inBattle = false;
            }
        }
        else
        {
            textBox.Visible = false;
            if (!playerIsAlive)
            {
                ShowGameOver();
            }
        }
        CombatCheck();
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
    
    private void HideGameOver()
    {
        gameOverScreen.Visible = false;
    }

    private void ShowGameOver()
    {
        gameOverScreen.Visible = true;
    }
}
